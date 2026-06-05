# Architecture

## Overview

MedCorVis is a modular monolith. Each module has its own Domain, Application,
Infrastructure, and Presentation layers. Module boundaries are designed so that
each module can be extracted independently without restructuring
its internal layers.

The API host (`MedCorVis.Api`) is responsible for startup wiring only.
It does not own any business logic or domain models.

## Project Structure

```
src/
  MedCorVis.Api                   # Host - middleware, startup, wiring only
  MedCorVis.Common                # Shared contracts, interfaces, and result types
  MedCorVis.Infrastructure        # Shared infrastructure (email via MailKit)
  MedCorVis.Modules.Identity      # Auth, JWT, refresh tokens, email confirmation
  MedCorVis.Modules.Users         # User profile management
  MedCorVis.Modules.Localization  # DB-backed translations, in-memory cache
  MedCorVis.Modules.CodeItems     # Healthcare reference data, multilingual labels

tests/
  MedCorVis.Modules.Identity.Tests
  MedCorVis.Modules.Users.Tests
  MedCorVis.Modules.Localization.Tests
  MedCorVis.Modules.CodeItems.Tests
```

## Module System

Each module implements `IModule` from `MedCorVis.Common`:

```csharp
public interface IModule
{
    IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration);
    WebApplication MapEndpoints(WebApplication app);
    Task RunStartupTasksAsync(WebApplication app) => Task.CompletedTask;
}
```

`Program.cs` calls `RegisterModules(...)` once, passing each module assembly.
The module system auto-discovers all `IModule` implementations, registers their
services, and adds their controllers as application parts. `Program.cs` does not
change when a new module is added.

`RunStartupTasksAsync` is where each module runs its own seeders.

Identity seeds roles and the admin account.
Localization seeds translations and warms the in-memory cache.
CodeItems seeds the healthcare vocabulary.

## Request Pipeline

```
UseHttpsRedirection
UseSerilogRequestLogging  
  ← must be before exception handling to capture full timing
UseMiddleware<ExceptionHandlingMiddleware>
UseAuthentication
UseMiddleware<CultureMiddleware>
  ← between auth and authorization
  ← resolves culture from JWT claim or Accept-Language header
UseAuthorization
```

`CultureMiddleware` resolves the request culture in this order:

1. If the caller is authenticated, read the user's preferred culture from the cache.
2. Otherwise, parse the `Accept-Language` header.
3. Fall back to English if no supported culture is found.

## Layer Conventions

Each module follows these layer rules:

- **Domain**: entities, value objects, domain exceptions. No framework dependencies.
- **Application**: services, interfaces, request/response contracts, error codes. No EF or infrastructure types.
- **Infrastructure**: EF DbContext, repositories, migrations, seeders. Implements application interfaces.
- **Presentation**: controllers. Calls application services only. No direct infrastructure access.

`*ServiceCollectionExtensions` classes are declared as `internal static`
to keep registration logic scoped to the module.

`InfrastructureServiceCollectionExtensions` is `public static`
because it is called by the host.

`*WebApplicationExtensions` classes are `public static`
because they are called by the host.

## Persistence

The application uses Entity Framework Core with SQL Server as the database provider.

Each module owns its own `DbContext`, migrations, and persistence configuration.

Database schema separation is handled per module using dedicated schemas
(e.g. `Identity`, `Localization`, `CodeItems`).

EF Core handles all database access.

## Error Handling

Services return `Result<T>` instead of throwing exceptions. `Result<T>` carries either
a value or a typed error. `BaseApiController.ToActionResult` maps the result to the
correct HTTP status code and an RFC 7807 ProblemDetails response body.

`ExceptionHandlingMiddleware` catches anything that escapes the service layer:

- `DomainException` maps to 422 with a machine-readable `code` field.
- Any other unhandled exception maps to 500.

All error responses include `traceId` and `code` extensions:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Not Found",
  "status": 404,
  "detail": "Translation not found.",
  "instance": "/api/v1/translations/99",
  "traceId": "...",
  "code": "LOCALIZATION_TRANSLATION_NOT_FOUND"
}
```
Error code format: `MODULE_RESOURCE_DESCRIPTION` (e.g. `CODEITEMS_ITEM_NOT_FOUND`).


Model validation errors (e.g. missing required fields, invalid date ranges) return 400
with a per-field breakdown:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/api/v1/code-items/categories/1/items",
  "traceId": "...",
  "errors": {
    "ValidFrom": ["ValidFrom must be before ValidTo."],
    "ValidTo":   ["ValidFrom must be before ValidTo."]
  }
}
```

## Cross-Module Communication

Modules do not reference each other's assemblies. 
Cross-module references use `Guid` only. No EF navigation properties across module boundaries.

`ICurrentUserService` (in `MedCorVis.Common`) is injected wherever business logic needs
the caller's identity. User ID is always resolved from the validated JWT token.
It is never accepted from a request body or URL parameter (IDOR prevention).
Nested routes enforce parent ownership at the service layer. A child resource is
only accessible when its stored parent ID matches the route (e.g. item.CategoryId
must equal the categoryId in the URL). This applies to all modules.

## Identity Module

`IIdentityUnitOfWork` is introduced in the Application layer to keep `IdentityDbContext`
out of `AuthService`. The service depends on the interface, not the EF context directly.
This keeps the service testable and the module portable.

Refresh token rotation uses SHA-256 hashing. The raw token is sent to the client;
only the hash is stored. SHA-256 is appropriate here because refresh tokens are
already cryptographically random (high-entropy), so bcrypt is unnecessary.

Each refresh token stores the IP address and user agent captured at creation time, 
and a RevokedAtUtc timestamp set when the token is revoked. 
This gives a per-session audit trail without a separate sessions table. 
ISessionContext in MedCorVis.Common abstracts the HTTP context read so AuthService stays free of framework types. 
The implementation in Identity infrastructure reads from IHttpContextAccessor. 
User agent strings are truncated to 500 characters at write time.

Token theft detection: if a revoked token is replayed and `ReplacedByTokenId` is set,
the system treats it as a stolen token and revokes the entire token family for the user.

A background service (`RefreshTokenCleanupService`) runs on a configurable interval to remove expired refresh tokens from the database.

`AccountService` owns all operations that touch `ApplicationUser` outside of auth:
culture preference, phone number, and the account deletion lifecycle. These are
kept in Identity because they operate directly on `ApplicationUser`.

The deletion workflow follows a request-and-approve pattern:

1. User submits a deletion request via `POST /users/me/deletion-request`.
2. Admin or MedicalSecretary reviews pending requests via `GET /users/deletion-requests`.
3. Staff executes deletion via `DELETE /users/{id}`.
4. On execution, PII fields are anonymised in both `Identity.Users` and `Profiles.Users`.
5. `BirthDate` is retained on the anonymised row for statistical purposes.

The module exposes three controllers:

- `AuthController`: registration, login, token refresh, logout, logout from all devices,
  email confirmation, resend confirmation email, password recovery (forgot-password,
  reset-password), and password change. Route: `/auth`.
- `AccountConsumerController`: self-service account endpoints for any authenticated user. Route: `/users`.
- `AccountController`: staff-facing account endpoints for Admin and MedicalSecretary.
  Manage pending deletion requests, execute deletion, and activate or deactivate accounts. Route: `/users`.

## Localization Module

`IMessageLocalizer` and `ILocalizerCache` are two separate interfaces. The email service
depends only on `IMessageLocalizer`. Cache warmup and admin refresh depend only on
`ILocalizerCache`. One implementation (`DbMessageLocalizer`) satisfies both.

Translations are stored in SQL Server (`Localization` schema) and loaded into an
in-memory cache on startup. The cache has no automatic expiry. It persists until
an admin triggers a reload via the cache refresh endpoint, or the API restarts.

The culture fallback chain is: `fr-CH → fr → en`.

The module exposes one controller:

- `LocalizationController`: admin CRUD for translations and cache management. Route: `/translations`.

## CodeItems Module

CodeItems is the application-wide healthcare controlled vocabulary. It is the single
source of truth for all healthcare classification terms used across modules
(appointment types, patient classifications, doctor roles, and more).

The module maintains its own translation table (`CodeItems.Translations`) separate from
`Localization.Translations`. The distinction is intentional:

- General translations (`Localization`) are stable, cache-friendly, and tolerate
  short staleness. They are loaded into an in-memory cache.
- Code item labels (`CodeItems`) are tied directly to domain data and must reflect
  admin changes immediately. They are never cached.

The module exposes two controllers:

- `CodeItemsController`: admin CRUD for categories, items, and translations. Route: `/code-items`.
- `CodeItemsConsumerController`: read-only consumer endpoint. Returns active items for a given category code with culture-resolved labels. Route: `GET /code-items/lookup/{categoryCode}`.

All item operations on nested routes (`/code-items/categories/{categoryId}/items/{id}`)
verify that the item's `CategoryId` matches the route parameter before proceeding.
A mismatch returns `404 Not Found` to avoid confirming the resource exists elsewhere.

Items and categories carry `IsSystemDefined`, `IsEditable`, and `IsDeletable` flags.
System-defined records are seeded and protected from accidental deletion. Admins can
create additional entries freely.

Items support an optional validity window via `ValidFrom` and `ValidTo` (`DateOnly?`).
The consumer endpoint filters out items outside their validity window at query time.
Items with no window set are always visible. Validity is admin-controlled and mutable.

`ValidDateRangeAttribute` in `MedCorVis.Common.Validations` handles cross-field date
range validation. Any request contract with a date range can use it.

## Users Module

The Users module owns profile data only. It has no dependency on the Identity project.

Profile data (`FirstName`, `LastName`, `BirthDate`) lives in `Profiles.Users` via the
`UserProfile` entity. `UserProfileService` is the only service in this module.
It implements `IUserProfileService` from `MedCorVis.Common`, which Identity also
consumes for profile reads and anonymisation during deletion.

The module exposes one controller:

- `UsersConsumerController`: self-service profile update for any authenticated user.
  Route: `PUT /users/me/profile`.

## API Versioning

API versioning is declared at the controller level (`[Route("api/v{version:apiVersion}/...")]`),
not globally in `Program.cs`. This keeps each module self-contained and portable
on extraction.

All routes are resource-based. They describe the resource, never the role or action
(e.g. `/translations`, not `/admin/translations`).

## Logging

Structured logging uses Serilog with a Seq sink. All log messages use the
`LoggerMessage.Define` pattern. Messages are compiled at startup, not on each call.

Log event ID ranges by module:

| Range     | Owner                                        |
|-----------|----------------------------------------------|
| 1000s     | Api (middleware)                             |
| 2001-2005 | Identity / AuthService / Register            |
| 2010-2014 | Identity / AuthService / Login               |
| 2020-2025 | Identity / AuthService / Refresh             |
| 2030-2032 | Identity / AuthService / Logout              |
| 2040-2043 | Identity / AuthService / EmailConfirmation   |
| 2050-2051 | Identity / AuthService / ResendConfirmation  |
| 2060-2062 | Identity / AuthService / RefreshTokenCleanup |
| 2070-2071 | Identity / AuthService / ForgotPassword      |
| 2080-2083 | Identity / AuthService / ResetPassword       |
| 2090-2092 | Identity / AuthService / ChangePassword      |
| 3001-3024 | Identity / AccountService                    |
| 3017-3018 | Users / UserProfileService                   |
| 4000s     | Localization                                 |
| 5001-5008 | Seeders (RoleSeeder, AdminUserSeeder)        |
| 6000s     | CodeItems                                    |
| 7000s     | Patients (next)                              |
| 8000s     | Next available after Patients                |

## Testing

Each service is tested in isolation. Repositories, `UserManager`, and other
infrastructure dependencies are substituted using NSubstitute.

Domain logic is tested directly against entity methods with no infrastructure involved.

`InternalsVisibleTo` is declared in each module's `.csproj`
for both the test project and `DynamicProxyGenAssembly2`.

Test projects mirror the source structure: one test class file per method group,
one base class per service that wires up the SUT and shared helpers.
Domain tests live under `Domain/` within each test project, separate from
service tests under `Application/Services/`.

## Author

Jerald James Capao, Software Engineer