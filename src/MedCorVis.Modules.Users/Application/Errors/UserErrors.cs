namespace MedCorVis.Modules.Users.Application.Errors;

using MedCorVis.Common.Results;

internal static class UserErrors
{
    public static readonly ResultError InvalidToken =
        new("USERS_INVALID_TOKEN", "Invalid or missing token.");

    public static readonly ResultError UserNotFound =
        new("USERS_USER_NOT_FOUND", "User not found.");
}