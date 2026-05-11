namespace MedCore.Modules.Localization.Application.Abstractions;

using MedCore.Common.Results;
using MedCore.Modules.Localization.Application.Contracts;

public interface ITranslationService
{
    Task<Result<IReadOnlyList<TranslationResponse>>> GetAllAsync(string? culture, CancellationToken ct = default);
    Task<Result<TranslationResponse>> GetByIdAsync(long id, CancellationToken ct = default);
    Task<Result<TranslationResponse>> CreateAsync(CreateTranslationRequest request, CancellationToken ct = default);
    Task<Result<TranslationResponse>> UpdateAsync(long id, UpdateTranslationRequest request, CancellationToken ct = default);
    Task<Result<bool>> DeleteAsync(long id, CancellationToken ct = default);
    Task RefreshCacheAsync(CancellationToken ct = default);
}