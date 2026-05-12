namespace MedCore.Modules.CodeItems.Application.Abstractions;

using MedCore.Common.Results;
using MedCore.Modules.CodeItems.Application.Contracts.Requests;
using MedCore.Modules.CodeItems.Application.Contracts.Responses;

public interface ICodeItemService
{
    // Categories
    Task<Result<IReadOnlyList<CategoryResponse>>> GetAllCategoriesAsync(CancellationToken ct = default);
    Task<Result<CategoryResponse>> GetCategoryByIdAsync(long id, CancellationToken ct = default);
    Task<Result<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct = default);
    Task<Result<CategoryResponse>> UpdateCategoryAsync(long id, UpdateCategoryRequest request, CancellationToken ct = default);
    Task<Result<bool>> ActivateCategoryAsync(long id, CancellationToken ct = default);
    Task<Result<bool>> DeactivateCategoryAsync(long id, CancellationToken ct = default);
    Task<Result<bool>> DeleteCategoryAsync(long id, CancellationToken ct = default);
    Task<Result<bool>> ReorderCategoriesAsync(ReorderRequest request, CancellationToken ct = default);

    // Items
    Task<Result<IReadOnlyList<ItemResponse>>> GetItemsByCategoryAsync(long categoryId, CancellationToken ct = default);
    Task<Result<ItemResponse>> GetItemByIdAsync(long id, CancellationToken ct = default);
    Task<Result<ItemResponse>> CreateItemAsync(long categoryId, CreateItemRequest request, CancellationToken ct = default);
    Task<Result<ItemResponse>> UpdateItemAsync(long id, UpdateItemRequest request, CancellationToken ct = default);
    Task<Result<bool>> ActivateItemAsync(long id, CancellationToken ct = default);
    Task<Result<bool>> DeactivateItemAsync(long id, CancellationToken ct = default);
    Task<Result<bool>> DeleteItemAsync(long id, CancellationToken ct = default);
    Task<Result<bool>> ReorderItemsAsync(long categoryId, ReorderRequest request, CancellationToken ct = default);

    // Translations
    Task<Result<IReadOnlyList<TranslationResponse>>> GetCategoryTranslationsAsync(long categoryId, CancellationToken ct = default);
    Task<Result<TranslationResponse>> UpsertCategoryTranslationAsync(long categoryId, string culture, UpsertTranslationRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<TranslationResponse>>> GetItemTranslationsAsync(long itemId, CancellationToken ct = default);
    Task<Result<TranslationResponse>> UpsertItemTranslationAsync(long itemId, string culture, UpsertTranslationRequest request, CancellationToken ct = default);

    // Consumer
    Task<Result<CodeItemListResponse>> GetActiveItemsAsync(string categoryCode, CancellationToken ct = default);
}