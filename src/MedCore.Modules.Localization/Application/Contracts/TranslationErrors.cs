namespace MedCore.Modules.Localization.Application.Contracts;

using MedCore.Common.Results;

internal static class TranslationErrors
{
    public static readonly ResultError NotFound =
        new("LOCALIZATION_TRANSLATION_NOT_FOUND", "Translation not found.");

    public static readonly ResultError DuplicateKey =
        new("LOCALIZATION_DUPLICATE_KEY", "A translation with this culture and key already exists.");

    public static readonly ResultError UnsupportedCulture =
        new("LOCALIZATION_UNSUPPORTED_CULTURE", "The specified culture is not supported.");

    public static readonly ResultError CreateFailed =
        new("LOCALIZATION_CREATE_FAILED", "Failed to create translation.");

    public static readonly ResultError UpdateFailed =
        new("LOCALIZATION_UPDATE_FAILED", "Failed to update translation.");

    public static readonly ResultError DeleteFailed =
        new("LOCALIZATION_DELETE_FAILED", "Failed to delete translation.");
}