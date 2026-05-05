namespace MedCore.Infrastructure.Localization;

using Microsoft.EntityFrameworkCore;

internal sealed class TranslationRepository : ITranslationRepository
{
    private readonly LocalizationDbContext _context;

    public TranslationRepository(LocalizationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Translation>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Translations
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsAsync(
        string culture, string key, CancellationToken ct = default)
    {
        return await _context.Translations
            .AnyAsync(t => t.Culture == culture && t.Key == key, ct);
    }

    public async Task AddAsync(Translation translation, CancellationToken ct = default)
    {
        await _context.Translations.AddAsync(translation, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}