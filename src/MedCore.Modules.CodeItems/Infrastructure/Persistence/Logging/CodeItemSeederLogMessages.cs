namespace MedCore.Modules.CodeItems.Infrastructure.Persistence.Logging;

using Microsoft.Extensions.Logging;

internal static class CodeItemSeederLogMessages
{
    public static readonly Action<ILogger, Exception?> SeedingStarted =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(6001, "CodeItemSeedingStarted"),
            "Seeding code items...");

    public static readonly Action<ILogger, int, int, int, Exception?> SeedingCompleted =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Information,
            new EventId(6002, "CodeItemSeedingCompleted"),
            "Code item seeding complete. Categories: {Categories}, Items: {Items}, Translations: {Translations}.");

    public static readonly Action<ILogger, string, Exception?> CategoryAlreadyExists =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(6003, "CodeItemCategoryAlreadyExists"),
            "Category '{Code}' already exists. Skipping.");

    public static readonly Action<ILogger, string, string, Exception?> ItemAlreadyExists =
        LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            new EventId(6004, "CodeItemAlreadyExists"),
            "Item '{Code}' in category '{Category}' already exists. Skipping.");

    public static readonly Action<ILogger, string, long, string, Exception?> TranslationAlreadyExists =
        LoggerMessage.Define<string, long, string>(
            LogLevel.Debug,
            new EventId(6005, "CodeItemTranslationAlreadyExists"),
            "Translation for EntityType '{EntityType}' EntityId {EntityId} culture '{Culture}' already exists. Skipping.");
}