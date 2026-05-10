namespace MedCore.Infrastructure.Localization;

using Microsoft.Extensions.Logging;

internal static class LocalizerLogMessages
{
    public static readonly Action<ILogger, string, string, Exception?> TranslationCacheEmpty =
        LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            new EventId(4001, "TranslationCacheEmpty"),
            "Translation cache is empty when resolving key '{Key}' for culture '{Culture}'. Returning key as fallback.");

    public static readonly Action<ILogger, string, string, Exception?> TranslationMissing =
        LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            new EventId(4002, "TranslationMissing"),
            "Missing translation for key '{Key}' in culture '{Culture}'. Returning key as fallback.");

    public static readonly Action<ILogger, int, Exception?> TranslationCacheLoaded =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(4003, "TranslationCacheLoaded"),
            "Translation cache loaded: {CultureCount} culture(s).");

    public static readonly Action<ILogger, Exception?> TranslationCacheInvalidated =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(4004, "TranslationCacheInvalidated"),
            "Translation cache invalidated.");
}