namespace MedCorVis.Modules.Identity.Application.Errors;

using MedCorVis.Common.Results;

internal static class AccountErrors
{
    public static readonly ResultError InvalidToken =
        new("IDENTITY_ACCOUNT_INVALID_TOKEN", "Invalid or missing token.");

    public static readonly ResultError UserNotFound =
        new("IDENTITY_ACCOUNT_USER_NOT_FOUND", "User not found.");

    public static readonly ResultError UnsupportedCulture =
        new("IDENTITY_ACCOUNT_UNSUPPORTED_CULTURE", "The specified culture is not supported.");

    public static readonly ResultError CultureUpdateFailed =
        new("IDENTITY_ACCOUNT_CULTURE_UPDATE_FAILED", "Failed to update preferred culture.");

    public static readonly ResultError PhoneUpdateFailed =
        new("IDENTITY_ACCOUNT_PHONE_UPDATE_FAILED", "Failed to update phone number.");

    public static readonly ResultError DeletionRequestAlreadyPending =
        new("IDENTITY_ACCOUNT_DELETION_REQUEST_ALREADY_PENDING", "A deletion request is already pending.");

    public static readonly ResultError NoDeletionRequestPending =
        new("IDENTITY_ACCOUNT_NO_DELETION_REQUEST_PENDING", "No pending deletion request found.");

    public static readonly ResultError DeletionFailed =
        new("IDENTITY_ACCOUNT_DELETION_FAILED", "Failed to process deletion.");

    public static readonly ResultError AlreadyDeleted =
        new("IDENTITY_ACCOUNT_ALREADY_DELETED", "User is already deleted.");
}