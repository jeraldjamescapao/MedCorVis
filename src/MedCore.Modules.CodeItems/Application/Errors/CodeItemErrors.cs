namespace MedCore.Modules.CodeItems.Application.Errors;

using MedCore.Common.Results;

internal static class CodeItemErrors
{
    // Category
    public static readonly ResultError CategoryNotFound =
        new("CODEITEMS_CATEGORY_NOT_FOUND", "Category not found.");

    public static readonly ResultError CategoryCodeAlreadyExists =
        new("CODEITEMS_CATEGORY_CODE_EXISTS", "A category with this code already exists.");

    public static readonly ResultError CategoryNotEditable =
        new("CODEITEMS_CATEGORY_NOT_EDITABLE", "This category cannot be edited.");

    public static readonly ResultError CategoryNotDeletable =
        new("CODEITEMS_CATEGORY_NOT_DELETABLE", "This category cannot be deleted.");

    public static readonly ResultError CategoryHasActiveItems =
        new("CODEITEMS_CATEGORY_HAS_ACTIVE_ITEMS", "Cannot delete a category that still has active items.");

    // Item
    public static readonly ResultError ItemNotFound =
        new("CODEITEMS_ITEM_NOT_FOUND", "Item not found.");

    public static readonly ResultError ItemCodeAlreadyExists =
        new("CODEITEMS_ITEM_CODE_EXISTS", "An item with this code already exists in this category.");

    public static readonly ResultError ItemNotEditable =
        new("CODEITEMS_ITEM_NOT_EDITABLE", "This item cannot be edited.");

    public static readonly ResultError ItemNotDeletable =
        new("CODEITEMS_ITEM_NOT_DELETABLE", "This item cannot be deleted.");

    // Translation
    public static readonly ResultError TranslationNotFound =
        new("CODEITEMS_TRANSLATION_NOT_FOUND", "Translation not found.");

    public static readonly ResultError UnsupportedCulture =
        new("CODEITEMS_UNSUPPORTED_CULTURE", "The specified culture is not supported.");
}