namespace MedCore.Infrastructure.Localization;

public interface ITranslationRepository
{
    Task<IReadOnlyList<Translation>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsAsync(string culture, string key, CancellationToken ct = default);
    Task AddAsync(Translation translation, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}