namespace MedCorVis.Modules.Identity.Infrastructure.Services;

using Microsoft.AspNetCore.Http;
using MedCorVis.Common.Services;
using MedCorVis.Modules.Identity.Domain.Tokens;

internal sealed class SessionContext : ISessionContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionContext(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public string? IpAddress =>
        _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public string? UserAgent
    {
        get
        {
            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
            if (string.IsNullOrEmpty(userAgent)) return null;
            
            return userAgent.Length > RefreshToken.UserAgentMaxLength
                ? userAgent[..RefreshToken.UserAgentMaxLength]
                : userAgent;
        }
    }
}