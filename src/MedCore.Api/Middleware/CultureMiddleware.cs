namespace MedCore.Api.Middleware;

using MedCore.Common.Caching;
using MedCore.Common.Localization;
using MedCore.Common.Services;
using MedCore.Modules.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;

internal sealed class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(
        HttpContext context,
        ICurrentCultureService currentCultureService,
        UserManager<ApplicationUser> userManager,
        IMemoryCache cache)
    {
        var culture = await ResolveAsync(context, userManager, cache);
        currentCultureService.SetCulture(culture);
        await _next(context);
    }
    
    private static async Task<string> ResolveAsync(
        HttpContext context,
        UserManager<ApplicationUser> userManager,
        IMemoryCache cache)
    {
        if (context.User.Identity?.IsAuthenticated != true)
            return ResolveFromHeader(context.Request.Headers.AcceptLanguage.ToString());
        
        var userIdClaim = context.User
            .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
            return ResolveFromHeader(context.Request.Headers.AcceptLanguage.ToString());
        
        var cacheKey = CacheKeys.UserCulture(userId);

        if (cache.TryGetValue(cacheKey, out string? cached) && cached is not null)
            return cached;

        var user = await userManager.FindByIdAsync(userId.ToString());
        var resolved = ResolveFromPreference(user?.PreferredCulture);

        cache.Set(cacheKey, resolved, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(30)
        });

        return resolved;
    }
    
    private static string ResolveFromPreference(string? preferredCulture)
    {
        if (preferredCulture is null)
            return SupportedCultures.Default;

        return SupportedCultures.All.Contains(preferredCulture)
            ? preferredCulture
            : SupportedCultures.Default;
    }
    
    private static string ResolveFromHeader(string acceptLanguage)
    {
        if (string.IsNullOrWhiteSpace(acceptLanguage))
            return SupportedCultures.Default;

        // "fr-CH,fr;q=0.9,en;q=0.8" → take the first tag before any quality weight
        var primary = acceptLanguage
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(tag => tag.Split(';')[0].Trim())
            .FirstOrDefault();

        if (primary is null)
            return SupportedCultures.Default;

        return SupportedCultures.All.Contains(primary)
            ? primary
            : SupportedCultures.Default;
    }
}