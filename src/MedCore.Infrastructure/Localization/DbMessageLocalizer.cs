namespace MedCore.Infrastructure.Localization;

using MedCore.Common.Caching;
using MedCore.Common.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

internal sealed class DbMessageLocalizer : IMessageLocalizer, ILocalizerCache
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMemoryCache _cache;
    private readonly ILogger<DbMessageLocalizer> _logger;
    
    public DbMessageLocalizer(
        IServiceScopeFactory scopeFactory,
        IMemoryCache cache,
        ILogger<DbMessageLocalizer> logger)
    {
        _scopeFactory = scopeFactory;
        _cache = cache;
        _logger = logger;
    }
    
    public string Get(string key, string culture)
    {
        var translations = _cache
            .Get<IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>>>(
                CacheKeys.Translations);

        if (translations is null)
        {
            _logger.LogWarning(
                "Translation cache is empty when resolving key '{Key}' for culture '{Culture}'. " +
                "Returning key as fallback.", key, culture);
            return key;
        }

        foreach (var candidate in BuildFallbackChain(culture))
        {
            if (translations.TryGetValue(candidate, out var dict) &&
                dict.TryGetValue(key, out var value))
                return value;
        }

        _logger.LogWarning(
            "Missing translation for key '{Key}' in culture '{Culture}'. Returning key as fallback.",
            key, culture);

        return key;
    }
    
    public async Task LoadAsync(CancellationToken ct = default)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();

        var repository = scope.ServiceProvider
            .GetRequiredService<ITranslationRepository>();

        var all = await repository.GetAllAsync(ct);

        var grouped = all
            .GroupBy(t => t.Culture)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyDictionary<string, string>)
                    g.ToDictionary(t => t.Key, t => t.Value));

        _cache.Set(
            CacheKeys.Translations,
            (IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>>)grouped);

        _logger.LogInformation(
            "Translation cache loaded: {EntryCount} entries across {CultureCount} culture(s).",
            all.Count, grouped.Count);
    }
    
    public void InvalidateCache()
    {
        _cache.Remove(CacheKeys.Translations);
        _logger.LogInformation("Translation cache invalidated.");
    }
    
    private static IEnumerable<string> BuildFallbackChain(string culture)
    {
        yield return culture;

        var dashIndex = culture.IndexOf('-');
        if (dashIndex > 0)
            yield return culture[..dashIndex];

        if (culture != SupportedCultures.Default)
            yield return SupportedCultures.Default;
    }
}