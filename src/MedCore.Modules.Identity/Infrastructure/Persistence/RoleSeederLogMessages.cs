namespace MedCore.Modules.Identity.Infrastructure.Persistence;

using Microsoft.Extensions.Logging;

internal static class RoleSeederLogMessages
{
    public static readonly Action<ILogger, string, Exception?> RoleSeedingStarted =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(5001, "RoleSeedingStarted"),
            "Seeding identity roles: {Roles}.");

    public static readonly Action<ILogger, string, Exception?> RoleAlreadyExists =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(5002, "RoleAlreadyExists"),
            "Role '{Role}' already exists. Skipping.");

    public static readonly Action<ILogger, string, Exception?> RoleSeededSuccessfully =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(5003, "RoleSeededSuccessfully"),
            "Role '{Role}' seeded successfully.");

    public static readonly Action<ILogger, string, string, Exception?> RoleSeedFailed =
        LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(5004, "RoleSeedFailed"),
            "Failed to seed role '{Role}': {Errors}.");
}