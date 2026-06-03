namespace MedCorVis.Modules.Identity.Presentation.Controllers;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedCorVis.Common.Account;
using MedCorVis.Common.Controllers;
using MedCorVis.Common.Results;
using MedCorVis.Common.Services;
using MedCorVis.Modules.Identity.Application.Errors;

[Authorize]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/users")]
public sealed class AccountConsumerController : BaseApiController
{
    private readonly IAccountService     _accountService;
    private readonly ICurrentUserService _currentUserService;

    public AccountConsumerController(
        IAccountService     accountService,
        ICurrentUserService currentUserService)
    {
        _accountService     = accountService;
        _currentUserService = currentUserService;
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> GetAccountAsync(CancellationToken ct)
    {
        if (!TryGetCurrentUserId(_currentUserService, out var userId))
            return ToActionResult(Result<AccountResponse>.Unauthorized(AccountErrors.InvalidToken));

        var result = await _accountService.GetAccountAsync(userId, ct);
        return ToActionResult(result);
    }
    
    [HttpPut("me/culture")]
    public async Task<IActionResult> UpdateCultureAsync(
        [FromBody] UpdateCultureRequest request, CancellationToken ct)
    {
        if (!TryGetCurrentUserId(_currentUserService, out var userId))
            return ToActionResult(Result<bool>.Unauthorized(AccountErrors.InvalidToken));

        var result = await _accountService.UpdateCultureAsync(userId, request.Culture, ct);
        return result.IsFailure ? ToActionResult(result) : NoContent();
    }
    
    [HttpPut("me/phone")]
    public async Task<IActionResult> UpdatePhoneAsync(
        [FromBody] UpdatePhoneRequest request, CancellationToken ct)
    {
        if (!TryGetCurrentUserId(_currentUserService, out var userId))
            return ToActionResult(Result<bool>.Unauthorized(AccountErrors.InvalidToken));

        var result = await _accountService.UpdatePhoneAsync(userId, request.PhoneNumber, ct);
        return result.IsFailure ? ToActionResult(result) : NoContent();
    }
    
    [HttpPost("me/deletion-request")]
    public async Task<IActionResult> RequestDeletionAsync(CancellationToken ct)
    {
        if (!TryGetCurrentUserId(_currentUserService, out var userId))
            return ToActionResult(Result<bool>.Unauthorized(AccountErrors.InvalidToken));

        var result = await _accountService.RequestDeletionAsync(userId, ct);
        return result.IsFailure ? ToActionResult(result) : NoContent();
    }
    
    [HttpDelete("me/deletion-request")]
    public async Task<IActionResult> CancelDeletionRequestAsync(CancellationToken ct)
    {
        if (!TryGetCurrentUserId(_currentUserService, out var userId))
            return ToActionResult(Result<bool>.Unauthorized(AccountErrors.InvalidToken));

        var result = await _accountService.CancelDeletionRequestAsync(userId, ct);
        return result.IsFailure ? ToActionResult(result) : NoContent();
    }
}