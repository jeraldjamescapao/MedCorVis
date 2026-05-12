namespace MedCore.Modules.Localization.Application.Logging;

using Microsoft.Extensions.Logging;

internal static class TranslationLogMessages
{
    public static readonly Action<ILogger, string, string, Exception?> TranslationCacheEmpty =
        LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            new EventId(4003, "TranslationCacheEmpty"),
            "Translation cache is empty when resolving key '{Key}' for culture '{Culture}'. Returning key as fallback.");

    public static readonly Action<ILogger, string, string, Exception?> TranslationMissing =
        LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            new EventId(4004, "TranslationMissing"),
            "Missing translation for key '{Key}' in culture '{Culture}'. Returning key as fallback.");

    public static readonly Action<ILogger, int, Exception?> TranslationCacheLoaded =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(4005, "TranslationCacheLoaded"),
            "Translation cache loaded: {CultureCount} culture(s).");

    public static readonly Action<ILogger, Exception?> TranslationCacheInvalidated =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(4006, "TranslationCacheInvalidated"),
            "Translation cache invalidated.");
    
    public static readonly Action<ILogger, long, Exception?> TranslationNotFound =
        LoggerMessage.Define<long>(
            LogLevel.Warning,
            new EventId(4007, "TranslationNotFound"),
            "Translation with ID {Id} was not found.");

    public static readonly Action<ILogger, string, string, Exception?> TranslationDuplicateKey =
        LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            new EventId(4008, "TranslationDuplicateKey"),
            "Duplicate translation key '{Key}' for culture '{Culture}'.");

    public static readonly Action<ILogger, string, string, Exception?> TranslationCreateFailed =
        LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(4009, "TranslationCreateFailed"),
            "Failed to create translation for culture '{Culture}' and key '{Key}'.");

    public static readonly Action<ILogger, long, Exception?> TranslationUpdateFailed =
        LoggerMessage.Define<long>(
            LogLevel.Error,
            new EventId(4010, "TranslationUpdateFailed"),
            "Failed to update translation with ID {Id}.");

    public static readonly Action<ILogger, long, Exception?> TranslationDeleteFailed =
        LoggerMessage.Define<long>(
            LogLevel.Error,
            new EventId(4011, "TranslationDeleteFailed"),
            "Failed to soft-delete translation with ID {Id}.");
}