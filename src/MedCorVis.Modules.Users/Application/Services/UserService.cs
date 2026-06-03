namespace MedCorVis.Modules.Users.Application.Services;

using Microsoft.Extensions.Logging;
using MedCorVis.Common.Results;
using MedCorVis.Modules.Users.Application.Abstractions;
using MedCorVis.Modules.Users.Application.Contracts.Requests;
using MedCorVis.Modules.Users.Application.Contracts.Responses;
using MedCorVis.Modules.Users.Application.Errors;
using MedCorVis.Modules.Users.Application.Logging;

internal sealed class UserService : IUserService
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly ILogger<UserService> _logger;
    
    public UserService(
        IUserProfileRepository userProfileRepository,
        ILogger<UserService> logger)
    {
        _userProfileRepository = userProfileRepository;
        _logger = logger;
    }
    
    public async Task<Result<UserProfileResponse>> UpdateProfileAsync(
        Guid userId, UpdateProfileRequest request, CancellationToken ct = default)
    {
        var profile = await _userProfileRepository.GetByUserIdAsync(userId, ct);
        if (profile is null)
        {
            UserLogMessages.UpdateProfileUserNotFound(_logger, userId, null);
            return Result<UserProfileResponse>.NotFound(UserErrors.UserNotFound);
        }

        profile.UpdateProfile(
            request.FirstName, request.LastName, request.BirthDate, userId.ToString());

        await _userProfileRepository.SaveChangesAsync(ct);

        UserLogMessages.UpdateProfileSucceeded(_logger, userId, null);

        return Result<UserProfileResponse>.Success(new UserProfileResponse(
            profile.FirstName,
            profile.LastName,
            profile.FullName,
            profile.BirthDate));
    }
}