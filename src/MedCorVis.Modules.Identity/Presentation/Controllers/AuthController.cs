namespace MedCorVis.Modules.Identity.Presentation.Controllers;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MedCorVis.Common.Controllers;
using MedCorVis.Common.Http;
using MedCorVis.Common.Results;
using MedCorVis.Common.Services;
using MedCorVis.Modules.Identity.Application.Abstractions.Authentication;
using MedCorVis.Modules.Identity.Application.Contracts.Authentication.Requests;
using MedCorVis.Modules.Identity.Application.Errors;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserService _currentUserService;

    public AuthController(
        IAuthService authService, 
        ICurrentUserService currentUserService)
    {
        _authService = authService;
        _currentUserService = currentUserService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterRequest request, CancellationToken ct)
    {
        var result = await _authService.RegisterAsync(request, ct);
        if (result.IsFailure) return ToActionResult(result);
        
        AppendRefreshTokenCookie(
            result.Value!.RawRefreshToken, result.Value.RefreshTokenExpiresAt);
        
        return ToActionResult(result, StatusCodes.Status201Created);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(request, ct);
        if (result.IsFailure) return ToActionResult(result);
        
        AppendRefreshTokenCookie(
            result.Value!.RawRefreshToken, result.Value.RefreshTokenExpiresAt);
        
        return ToActionResult(result);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync(CancellationToken ct)
    {
        var token = Request.Cookies[CookieNames.RefreshToken] ?? string.Empty;
        var result = await _authService.RefreshAsync(token, ct);
        if (result.IsFailure) return ToActionResult(result);
        
        AppendRefreshTokenCookie(
            result.Value!.RawRefreshToken, result.Value.RefreshTokenExpiresAt);
        
        return ToActionResult(result);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync(CancellationToken ct)
    {
        var token = Request.Cookies[CookieNames.RefreshToken] ?? string.Empty;
        var result = await _authService.LogoutAsync(token, ct);
        if (result.IsFailure) return ToActionResult(result);
        
        DeleteRefreshTokenCookie();
        return NoContent();
    }

    [Authorize]
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAllAsync(CancellationToken ct)
    {
        if (!TryGetCurrentUserId(_currentUserService, out var userId))
            return ToActionResult(Result<bool>.Unauthorized(AuthErrors.InvalidCredentials));
        
        var result = await _authService.LogoutAllAsync(userId, ct);
        if (result.IsFailure) return ToActionResult(result);
        
        DeleteRefreshTokenCookie();
        return NoContent();
    }
    
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync(
        [FromQuery] Guid userId,
        [FromQuery] string token,
        CancellationToken ct)
    {
        var result = await _authService.ConfirmEmailAsync(userId, token, ct);
        if (result.IsFailure) return ToActionResult(result);
        
        return NoContent();
    }

    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmailAsync(
        [FromBody] ResendConfirmationEmailRequest request,
        CancellationToken ct)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request.Email, ct);
        if (result.IsFailure) return ToActionResult(result);

        return NoContent();
    }
    
    private void AppendRefreshTokenCookie(string rawRefreshToken, DateTimeOffset expiresAt)
    {
        Response.Cookies.Append(CookieNames.RefreshToken, rawRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = GetVersionedPath("auth"),
            Expires = expiresAt
        });
    }

    private void DeleteRefreshTokenCookie()
    {
        Response.Cookies.Delete(CookieNames.RefreshToken, new CookieOptions
        {
            Path = GetVersionedPath("auth")
        });
    }
}