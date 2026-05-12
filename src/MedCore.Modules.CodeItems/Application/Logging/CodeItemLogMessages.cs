namespace MedCore.Modules.CodeItems.Application.Logging;

using Microsoft.Extensions.Logging;

internal static class CodeItemLogMessages
{
    public static readonly Action<ILogger, string, Exception?> CategoryNotFound =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(6006, "CodeItemCategoryNotFound"),
            "Category '{Code}' not found.");

    public static readonly Action<ILogger, long, Exception?> CategoryNotFoundById =
        LoggerMessage.Define<long>(
            LogLevel.Warning,
            new EventId(6007, "CodeItemCategoryNotFoundById"),
            "Category with id {Id} not found.");

    public static readonly Action<ILogger, string, Exception?> CategoryCodeAlreadyExists =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(6008, "CodeItemCategoryCodeAlreadyExists"),
            "Category code '{Code}' already exists.");

    public static readonly Action<ILogger, long, Exception?> ItemNotFound =
        LoggerMessage.Define<long>(
            LogLevel.Warning,
            new EventId(6009, "CodeItemItemNotFound"),
            "Item with id {Id} not found.");

    public static readonly Action<ILogger, string, long, Exception?> ItemCodeAlreadyExists =
        LoggerMessage.Define<string, long>(
            LogLevel.Warning,
            new EventId(6010, "CodeItemItemCodeAlreadyExists"),
            "Item code '{Code}' already exists in category {CategoryId}.");

    public static readonly Action<ILogger, long, Exception?> TranslationNotFound =
        LoggerMessage.Define<long>(
            LogLevel.Warning,
            new EventId(6011, "CodeItemTranslationNotFound"),
            "Translation with id {Id} not found.");
}