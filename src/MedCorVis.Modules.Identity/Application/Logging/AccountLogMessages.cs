namespace MedCorVis.Modules.Identity.Application.Logging;

using Microsoft.Extensions.Logging;

internal static class AccountLogMessages
{
    #region Get Account

    public static readonly Action<ILogger, Guid, Exception?> GetAccountNotFound =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(3001, "GetAccountNotFound"),
            "Account retrieval failed: user {UserId} not found.");

    public static readonly Action<ILogger, Guid, Exception?> GetAccountSucceeded =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(3002, "GetAccountSucceeded"),
            "Account retrieved successfully for user {UserId}.");

    #endregion

    #region Update Culture

    public static readonly Action<ILogger, Guid, string, Exception?> UpdateCultureUnsupported =
        LoggerMessage.Define<Guid, string>(
            LogLevel.Warning,
            new EventId(3003, "UpdateCultureUnsupported"),
            "Culture update rejected for user {UserId}: unsupported culture {Culture}.");

    public static readonly Action<ILogger, Guid, Exception?> UpdateCultureUserNotFound =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(3004, "UpdateCultureUserNotFound"),
            "Culture update failed: user {UserId} not found.");

    public static readonly Action<ILogger, Guid, Exception?> UpdateCultureFailed =
        LoggerMessage.Define<Guid>(
            LogLevel.Error,
            new EventId(3005, "UpdateCultureFailed"),
            "Culture update failed for user {UserId}: identity update returned errors.");

    public static readonly Action<ILogger, Guid, string, Exception?> UpdateCultureSucceeded =
        LoggerMessage.Define<Guid, string>(
            LogLevel.Information,
            new EventId(3006, "UpdateCultureSucceeded"),
            "Preferred culture updated to {Culture} for user {UserId}.");

    #endregion

    #region Update Phone

    public static readonly Action<ILogger, Guid, Exception?> UpdatePhoneUserNotFound =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(3007, "UpdatePhoneUserNotFound"),
            "Phone update failed: user {UserId} not found.");

    public static readonly Action<ILogger, Guid, Exception?> UpdatePhoneFailed =
        LoggerMessage.Define<Guid>(
            LogLevel.Error,
            new EventId(3008, "UpdatePhoneFailed"),
            "Phone update failed for user {UserId}: identity update returned errors.");

    public static readonly Action<ILogger, Guid, Exception?> UpdatePhoneSucceeded =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(3009, "UpdatePhoneSucceeded"),
            "Phone number updated successfully for user {UserId}.");

    #endregion

    #region Deletion Request

    public static readonly Action<ILogger, Guid, Exception?> DeletionRequestAlreadyPending =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(3010, "DeletionRequestAlreadyPending"),
            "Deletion request rejected for user {UserId}: a request is already pending.");

    public static readonly Action<ILogger, Guid, Exception?> DeletionRequestSubmitted =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(3011, "DeletionRequestSubmitted"),
            "User {UserId} submitted a deletion request.");

    public static readonly Action<ILogger, Guid, Exception?> NoDeletionRequestPending =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(3012, "NoDeletionRequestPending"),
            "Deletion request cancellation failed for user {UserId}: no pending request found.");

    public static readonly Action<ILogger, Guid, Exception?> DeletionRequestCancelled =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(3013, "DeletionRequestCancelled"),
            "User {UserId} cancelled their deletion request.");

    #endregion

    #region Execute Deletion

    public static readonly Action<ILogger, Guid, Exception?> UserAlreadyDeleted =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(3014, "UserAlreadyDeleted"),
            "Deletion failed: user {UserId} is already deleted.");

    public static readonly Action<ILogger, Guid, Exception?> UserDeletionFailed =
        LoggerMessage.Define<Guid>(
            LogLevel.Error,
            new EventId(3015, "UserDeletionFailed"),
            "Deletion failed for user {UserId}: identity update returned errors.");

    public static readonly Action<ILogger, Guid, Guid, Exception?> UserDeletedSuccessfully =
        LoggerMessage.Define<Guid, Guid>(
            LogLevel.Information,
            new EventId(3016, "UserDeletedSuccessfully"),
            "User {TargetUserId} deleted and anonymised by actor {ActorId}.");

    #endregion
}