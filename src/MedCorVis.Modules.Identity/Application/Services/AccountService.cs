namespace MedCorVis.Modules.Identity.Application.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MedCorVis.Common.Account;
using MedCorVis.Common.Caching;
using MedCorVis.Common.Localization;
using MedCorVis.Common.Results;
using MedCorVis.Common.UserProfiles;
using MedCorVis.Modules.Identity.Application.Errors;
using MedCorVis.Modules.Identity.Application.Logging;
using MedCorVis.Modules.Identity.Domain.Users;

internal sealed class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserProfileService          _userProfileService;
    private readonly IUserCultureCache            _userCultureCache;
    private readonly ILogger<AccountService>      _logger;

    public AccountService(
        UserManager<ApplicationUser> userManager,
        IUserProfileService userProfileService,
        IUserCultureCache userCultureCache,
        ILogger<AccountService> logger)
    {
        _userManager = userManager;
        _userProfileService = userProfileService;
        _userCultureCache = userCultureCache;
        _logger = logger;
    }
    
    public async Task<Result<AccountResponse>> GetAccountAsync(
        Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            AccountLogMessages.GetAccountNotFound(_logger, userId, null);
            return Result<AccountResponse>.NotFound(AccountErrors.UserNotFound);
        }

        var profile = await _userProfileService.GetProfileAsync(userId, ct);
        if (profile is null)
        {
            AccountLogMessages.GetAccountNotFound(_logger, userId, null);
            return Result<AccountResponse>.NotFound(AccountErrors.UserNotFound);
        }

        AccountLogMessages.GetAccountSucceeded(_logger, userId, null);
        return Result<AccountResponse>.Success(MapToResponse(user, profile));
    }
    
    public async Task<Result<bool>> UpdateCultureAsync(
        Guid userId, string culture, CancellationToken ct = default)
    {
        if (!SupportedCultures.All.Contains(culture))
        {
            AccountLogMessages.UpdateCultureUnsupported(_logger, userId, culture, null);
            return Result<bool>.Validation(AccountErrors.UnsupportedCulture);
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            AccountLogMessages.UpdateCultureUserNotFound(_logger, userId, null);
            return Result<bool>.NotFound(AccountErrors.UserNotFound);
        }

        user.UpdatePreferredCulture(culture, userId.ToString());

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            AccountLogMessages.UpdateCultureFailed(_logger, userId, null);
            return Result<bool>.Internal(AccountErrors.CultureUpdateFailed);
        }

        _userCultureCache.SetCultureForUser(userId, culture);
        AccountLogMessages.UpdateCultureSucceeded(_logger, userId, culture, null);
        return Result<bool>.Success(true);
    }
    
    public async Task<Result<bool>> UpdatePhoneAsync(
        Guid userId, string? phoneNumber, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            AccountLogMessages.UpdatePhoneUserNotFound(_logger, userId, null);
            return Result<bool>.NotFound(AccountErrors.UserNotFound);
        }

        var result = await _userManager.SetPhoneNumberAsync(user, phoneNumber);
        if (!result.Succeeded)
        {
            AccountLogMessages.UpdatePhoneFailed(_logger, userId, null);
            return Result<bool>.Internal(AccountErrors.PhoneUpdateFailed);
        }

        AccountLogMessages.UpdatePhoneSucceeded(_logger, userId, null);
        return Result<bool>.Success(true);
    }
    
    public async Task<Result<bool>> RequestDeletionAsync(
        Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            AccountLogMessages.GetAccountNotFound(_logger, userId, null);
            return Result<bool>.NotFound(AccountErrors.UserNotFound);
        }

        if (user.DeletionRequestedAtUtc.HasValue)
        {
            AccountLogMessages.DeletionRequestAlreadyPending(_logger, userId, null);
            return Result<bool>.Conflict(AccountErrors.DeletionRequestAlreadyPending);
        }

        user.RequestDeletion();

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            AccountLogMessages.UserDeletionFailed(_logger, userId, null);
            return Result<bool>.Internal(AccountErrors.DeletionFailed);
        }

        AccountLogMessages.DeletionRequestSubmitted(_logger, userId, null);
        return Result<bool>.Success(true);
    }
    
    public async Task<Result<bool>> CancelDeletionRequestAsync(
        Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            AccountLogMessages.GetAccountNotFound(_logger, userId, null);
            return Result<bool>.NotFound(AccountErrors.UserNotFound);
        }

        if (!user.DeletionRequestedAtUtc.HasValue)
        {
            AccountLogMessages.NoDeletionRequestPending(_logger, userId, null);
            return Result<bool>.UnprocessableEntity(AccountErrors.NoDeletionRequestPending);
        }

        user.CancelDeletionRequest();

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            AccountLogMessages.UserDeletionFailed(_logger, userId, null);
            return Result<bool>.Internal(AccountErrors.DeletionFailed);
        }

        AccountLogMessages.DeletionRequestCancelled(_logger, userId, null);
        return Result<bool>.Success(true);
    }
    
    public async Task<Result<IReadOnlyList<DeletionRequestResponse>>> GetPendingDeletionRequestsAsync(
        CancellationToken ct = default)
    {
        var pendingUsers = await _userManager.Users
            .AsNoTracking()
            .Where(u => u.DeletionRequestedAtUtc != null)
            .OrderBy(u => u.DeletionRequestedAtUtc)
            .ToListAsync(ct);

        if (pendingUsers.Count == 0)
            return Result<IReadOnlyList<DeletionRequestResponse>>.Success([]);

        var responses = new List<DeletionRequestResponse>(pendingUsers.Count);

        foreach (var user in pendingUsers)
        {
            var profile = await _userProfileService.GetProfileAsync(user.Id, ct);
            if (profile is null) continue;

            responses.Add(new DeletionRequestResponse(
                user.Id,
                profile.FullName,
                user.Email!,
                user.DeletionRequestedAtUtc!.Value));
        }

        return Result<IReadOnlyList<DeletionRequestResponse>>.Success(responses);
    }
    
    public async Task<Result<bool>> ExecuteDeletionAsync(
        Guid actorId, Guid targetUserId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(targetUserId.ToString());
        if (user is null)
        {
            AccountLogMessages.GetAccountNotFound(_logger, targetUserId, null);
            return Result<bool>.NotFound(AccountErrors.UserNotFound);
        }

        if (user.IsDeleted)
        {
            AccountLogMessages.UserAlreadyDeleted(_logger, targetUserId, null);
            return Result<bool>.UnprocessableEntity(AccountErrors.AlreadyDeleted);
        }

        user.Delete(actorId.ToString());

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            AccountLogMessages.UserDeletionFailed(_logger, targetUserId, null);
            return Result<bool>.Internal(AccountErrors.DeletionFailed);
        }

        await _userProfileService.AnonymiseProfileAsync(targetUserId, actorId.ToString(), ct);

        AccountLogMessages.UserDeletedSuccessfully(_logger, targetUserId, actorId, null);
        return Result<bool>.Success(true);
    }
    
    public async Task<Result<bool>> ActivateUserAsync(
        Guid actorId, Guid targetUserId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(targetUserId.ToString());
        if (user is null)
        {
            AccountLogMessages.ActivateUserNotFound(_logger, targetUserId, null);
            return Result<bool>.NotFound(AccountErrors.UserNotFound);
        }

        if (user.IsActive)
        {
            AccountLogMessages.UserAlreadyActive(_logger, targetUserId, null);
            return Result<bool>.Conflict(AccountErrors.AlreadyActive);
        }

        user.Activate(actorId.ToString());

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            AccountLogMessages.UserDeletionFailed(_logger, targetUserId, null);
            return Result<bool>.Internal(AccountErrors.DeletionFailed);
        }

        AccountLogMessages.UserActivated(_logger, targetUserId, actorId, null);
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeactivateUserAsync(
        Guid actorId, Guid targetUserId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(targetUserId.ToString());
        if (user is null)
        {
            AccountLogMessages.DeactivateUserNotFound(_logger, targetUserId, null);
            return Result<bool>.NotFound(AccountErrors.UserNotFound);
        }

        if (!user.IsActive)
        {
            AccountLogMessages.UserAlreadyInactive(_logger, targetUserId, null);
            return Result<bool>.Conflict(AccountErrors.AlreadyInactive);
        }

        user.Deactivate(actorId.ToString());

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            AccountLogMessages.UserDeletionFailed(_logger, targetUserId, null);
            return Result<bool>.Internal(AccountErrors.DeletionFailed);
        }

        AccountLogMessages.UserDeactivated(_logger, targetUserId, actorId, null);
        return Result<bool>.Success(true);
    }
    
    private static AccountResponse MapToResponse(ApplicationUser user, UserProfileData profile)
    {
        return new AccountResponse(
            user.Id,
            user.Email!,
            profile.FirstName,
            profile.LastName,
            profile.FullName,
            profile.BirthDate,
            user.PreferredCulture,
            user.PhoneNumber,
            user.IsActive,
            user.IsDeleted,
            user.DeletionRequestedAtUtc,
            user.CreatedAtUtc,
            user.ModifiedAtUtc);
    }
}