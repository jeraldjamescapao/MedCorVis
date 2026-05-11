# MedCore

MedCore is a healthcare API for managing patients, doctors, and appointments.
Built as a portfolio project showcasing .NET 10 modular monolith architecture
with clean layer separation, JWT authentication, refresh token rotation,
and structured logging. Built with ASP.NET Core, EF Core, and SQL Server.

## The Story

I built MedCore to show how I approach backend development. The domain is healthcare:
patients, doctors, and appointments. Straightforward enough to stay focused, but
with enough moving parts to make real architectural decisions matter. I wanted a codebase where every decision has a reason I can defend, not just code that runs.

## What is built so far

### Identity Module

- JWT authentication with refresh token rotation and theft detection
- SHA-256 refresh token hashing stored in SQL Server
- HttpOnly cookie-based token delivery
- Email confirmation flow via MailKit (Ethereal Email for development)
- Role-based access control (Admin, Patient, Doctor)
- Structured logging with Serilog
- RFC 7807 ProblemDetails error responses
- Background service for expired token cleanup

### Users Module

- User ID resolved from the JWT token. Never accepted from the URL to prevent Insecure Direct Object Reference (IDOR).
- Profile data includes name, birth date, preferred culture, and account status
- `GET /api/v1/users/me`: authenticated users can retrieve their own account profile
- `PUT /api/v1/users/me/culture`: authenticated users can update their preferred language
- `PUT /api/v1/users/me/profile`: authenticated users can update their name and birth date
  > Note: in clinical systems, name and birth date changes are identity-critical and typically require admin approval. In MedCore, self-service edits are permitted at the account level. Clinical records (future Patients module) will enforce stricter controls.
- `PUT /api/v1/users/me/phone`: authenticated users can update their phone number (confirmation via SMS is planned, not yet implemented)

### Localization Module

- DB-backed translations stored in the `Localization` schema
- Supported cultures: `en`, `fr`, `de`, `fr-CH`, `de-CH`
- Culture resolution per request: authenticated users resolved from their stored
  preference, unauthenticated users resolved from the `Accept-Language` header,
  fallback to `en`
- Preferred culture cached per user with a 30-minute sliding expiry
- Confirmation emails delivered in the user's resolved language
- `POST /api/v1/admin/translations/cache/refresh`: Admin only, reloads translation cache
- `GET /api/v1/admin/translations`: returns all translations, optionally filtered by `?culture=`
- `GET /api/v1/admin/translations/{id}`: returns a single translation by ID
- `POST /api/v1/admin/translations`: creates a new translation
- `PUT /api/v1/admin/translations/{id}`: updates the value and description of a translation
- `DELETE /api/v1/admin/translations/{id}`: soft-deletes a translation; preserves audit trail

### Tests

44 unit tests across two projects using xUnit, NSubstitute, and FluentAssertions.

**AuthService**: 31 tests covering registration, login, token refresh, logout,
logout-all, email confirmation, and confirmation resend (including reuse detection
and full family revocation on token theft).

**UserService**: 13 tests covering profile retrieval, culture update, profile update,
and phone update.

## Architecture

Modular monolith with clean layer separation: Domain, Application,
Infrastructure, and Presentation. Module boundaries are designed so
each module can be extracted into a standalone microservice.

Each module registers its own services, persistence, and controllers.
The API host only handles startup wiring.

`IIdentityUnitOfWork` is introduced in the Application layer to keep
`IdentityDbContext` out of `AuthService`. It enforces the Dependency
Inversion Principle and makes the service testable without a real database,
even though it is overkill at this scale. The intent is to show the module
can be extracted without restructuring the service layer.

`IMessageLocalizer` and `ILocalizerCache` are separated into two interfaces
following the Interface Segregation Principle. Email service depends only on
`IMessageLocalizer`. Cache warmup on startup and admin refresh depend only on
`ILocalizerCache`. One implementation (`DbMessageLocalizer`) satisfies both.

The Users module accesses `ApplicationUser` via `UserManager<ApplicationUser>` and shares
the `Identity.Users` table with the Identity module. This is a known tradeoff in modular
monoliths. Identity owns credentials and tokens. Users owns profile data. On extraction,
Identity publishes a `UserRegisteredEvent` and Users maintains its own copy of profile
data in a separate database.

The data layer uses EF Core with SQL Server as the provider. Switching providers
requires updating `UseSqlServer` in each `DbContext` registration and regenerating
migrations. SQL Server is the standard in enterprise .NET environments.

## Tech Stack

- ASP.NET Core (.NET 10)
- SQL Server 2022 (Docker)
- Entity Framework Core 10
- ASP.NET Core Identity
- Serilog
- MailKit
- Seq (structured log viewer, Docker)

## Getting Started

### Prerequisites

- .NET 10 SDK
- Docker Desktop
- dotnet-ef CLI (`dotnet tool install --global dotnet-ef`)
- Trusted HTTPS dev certificate (`dotnet dev-certs https --trust`)

#### Apple Silicon (M1/M2/M3/M4/M5)

The official SQL Server Docker image does not support ARM64.
Use `azure-sql-edge` as a drop-in replacement. In `docker-compose.yml`, replace:

```yaml
image: mcr.microsoft.com/mssql/server:2022-latest
```

with:

```yaml
image: mcr.microsoft.com/azure-sql-edge
```

### Run the services

```bash
docker-compose up -d
```

SQL Server will be available at `localhost:1433`.
Seq will be available at `http://localhost:5341`.

### Apply migrations

From the solution root:

Apply Identity migrations:

```bash
dotnet ef database update --project src/MedCore.Modules.Identity/MedCore.Modules.Identity.csproj --startup-project src/MedCore.Api/MedCore.Api.csproj --context IdentityDbContext
```

Apply Localization migrations:

```bash
dotnet ef database update --project src/MedCore.Modules.Localization/MedCore.Modules.Localization.csproj --startup-project src/MedCore.Api/MedCore.Api.csproj --context LocalizationDbContext
```

### Run the API

From the solution root:

```bash
dotnet run --project src/MedCore.Api/MedCore.Api.csproj --launch-profile https
```

The API will be available at `https://localhost:7212`.

On startup, the app seeds roles, the default admin account, and translations automatically,
then loads the translation cache before accepting requests.

### Run the tests

From the solution root:

```bash
dotnet test
```

### API docs

The API is versioned. All endpoints are available under `/api/v1/`.

With the API running, visit: https://localhost:7212/scalar/v1

### Structured logs (Seq)

Visit `http://localhost:5341` to browse and query structured logs in real time.

### Try the API with MedCore.http

The repo includes `src/MedCore.Api/MedCore.http` with all endpoints
pre-configured and ready to run. It works in:

- **JetBrains Rider**: built-in HTTP client, no setup needed
- **Visual Studio**: built-in HTTP client, no setup needed
- **VS Code**: install the [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) extension by Huachao Mao

Open the file, start the API first, then set `@AccessToken` to a token from a login or register response and run any request directly from your editor.

### Email (development)

The app uses Ethereal Email for development. No real emails are delivered.
Credentials in `appsettings.json` are intentional for reviewer convenience.
To use your own Ethereal account, create a free one at https://ethereal.email
and override the `Email` section in `appsettings.Development.json`.

### Default admin account

A default admin account is seeded on first startup for reviewer convenience:

- **Email**: admin@medcore.dev
- **Password**: Admin_MedCore_2024!

Use this account to log in and test the translation management endpoints.

## Status

Actively in development. Identity module, Users module, and Localization module are complete.
Identity and Users modules have unit test coverage. CodeItems, Patients, Doctors,
and Appointments modules are next.

## About the Author

Jerald James Capao — Software Engineer

GitHub: https://github.com/jeraldjamescapao

Architecture, domain modeling, and technical decisions are all my own.
AI tools such as Anthropic Claude are used as support during development.