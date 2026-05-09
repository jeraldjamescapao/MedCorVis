namespace MedCore.Api.Middleware;

using MedCore.Common.Localization;
using MedCore.Common.Services;

internal sealed class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(
        HttpContext context,
        ICurrentUserService currentUserService,
        ICurrentCultureService currentCultureService,
        ICultureResolver cultureResolver)
    {
        var culture = await ResolveAsync(
            currentUserService, 
            cultureResolver, 
            context.Request.Headers.AcceptLanguage.ToString(), 
            context.RequestAborted);
        
        currentCultureService.SetCulture(culture);
        
        await _next(context);
    }
    
    private static async Task<string> ResolveAsync(
        ICurrentUserService currentUserService,
        ICultureResolver cultureResolver,
        string acceptLanguage,
        CancellationToken ct)
    {
        if (!currentUserService.IsAuthenticated || 
            !Guid.TryParse(currentUserService.UserId, out var userId))
            return ResolveFromHeader(acceptLanguage);

        return await cultureResolver.ResolveForUserAsync(userId, ct);
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