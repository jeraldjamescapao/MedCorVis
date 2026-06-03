namespace MedCorVis.Modules.Users.Application.Logging;

using Microsoft.Extensions.Logging;

internal static class UserLogMessages
{
    public static readonly Action<ILogger, Guid, Exception?> UpdateProfileUserNotFound =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(3017, "UpdateProfileUserNotFound"),
            "Profile update failed: user {UserId} not found.");

    public static readonly Action<ILogger, Guid, Exception?> UpdateProfileSucceeded =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(3018, "UpdateProfileSucceeded"),
            "Profile updated successfully for user {UserId}.");
}