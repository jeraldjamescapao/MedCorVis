namespace MedCore.Modules.Localization.Infrastructure.Persistence;

using Microsoft.Extensions.Logging;

internal static class TranslationSeederLogMessages
{
    public static readonly Action<ILogger, Exception?> TranslationSeedingStarted =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(4001, "TranslationSeedingStarted"),
            "Seeding translations...");

    public static readonly Action<ILogger, int, int, Exception?> TranslationSeedingCompleted =
        LoggerMessage.Define<int, int>(
            LogLevel.Information,
            new EventId(4002, "TranslationSeedingCompleted"),
            "Translation seeding complete. Seeded: {Seeded}, Skipped: {Skipped}.");
}