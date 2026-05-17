namespace MedCorVis.Modules.Identity.Infrastructure.Services;

using Microsoft.AspNetCore.Http;
using MedCorVis.Common.Authorization;
using MedCorVis.Common.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;    
    }
    
    public string UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated != true) return SystemActors.System;
            return user.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? SystemActors.System;
        }
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}