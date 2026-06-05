namespace MedCorVis.Modules.Identity.Presentation.Controllers;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedCorVis.Common.Account;
using MedCorVis.Common.Authorization;
using MedCorVis.Common.Controllers;
using MedCorVis.Common.Results;
using MedCorVis.Common.Services;
using MedCorVis.Modules.Identity.Application.Errors;

[Authorize(Roles = $"{AppRoles.Admin}, {AppRoles.MedicalSecretary}")]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/users")]
public sealed class AccountController : BaseApiController
{
    private readonly IAccountService     _accountService;
    private readonly ICurrentUserService _currentUserService;

    public AccountController(
        IAccountService     accountService,
        ICurrentUserService currentUserService)
    {
        _accountService     = accountService;
        _currentUserService = currentUserService;
    }
    
    [HttpGet("deletion-requests")]
    public async Task<IActionResult> GetPendingDeletionRequestsAsync(CancellationToken ct)
    {
        var result = await _accountService.GetPendingDeletionRequestsAsync(ct);
        return ToActionResult(result);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> ExecuteDeletionAsync(Guid id, CancellationToken ct)
    {
        if (!TryGetCurrentUserId(_currentUserService, out var actorId))
            return ToActionResult(Result<bool>.Unauthorized(AccountErrors.InvalidToken));

        var result = await _accountService.ExecuteDeletionAsync(actorId, id, ct);
        return result.IsFailure ? ToActionResult(result) : NoContent();
    }
    
    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> ActivateUserAsync(Guid id, CancellationToken ct)
    {
        if (!TryGetCurrentUserId(_currentUserService, out var actorId))
            return ToActionResult(Result<bool>.Unauthorized(AccountErrors.InvalidToken));

        var result = await _accountService.ActivateUserAsync(actorId, id, ct);
        return result.IsFailure ? ToActionResult(result) : NoContent();
    }

    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateUserAsync(Guid id, CancellationToken ct)
    {
        if (!TryGetCurrentUserId(_currentUserService, out var actorId))
            return ToActionResult(Result<bool>.Unauthorized(AccountErrors.InvalidToken));

        var result = await _accountService.DeactivateUserAsync(actorId, id, ct);
        return result.IsFailure ? ToActionResult(result) : NoContent();
    }
}