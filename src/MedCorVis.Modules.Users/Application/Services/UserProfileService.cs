namespace MedCorVis.Modules.Users.Application.Services;

using Microsoft.Extensions.Logging;
using MedCorVis.Common.Results;
using MedCorVis.Common.UserProfiles;
using MedCorVis.Modules.Users.Application.Abstractions;
using MedCorVis.Modules.Users.Application.Errors;
using MedCorVis.Modules.Users.Application.Logging;
using MedCorVis.Modules.Users.Domain;

internal sealed class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _repository;
    private readonly ILogger<UserProfileService> _logger;

    public UserProfileService(
        IUserProfileRepository repository,
        ILogger<UserProfileService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task CreateProfileAsync(
        Guid userId,
        string firstName,
        string lastName,
        DateOnly birthDate,
        string createdBy,
        CancellationToken ct = default)
    {
        var profile = UserProfile.Create(
            userId, firstName, lastName, birthDate, createdBy);

        await _repository.AddAsync(profile, ct);
        await _repository.SaveChangesAsync(ct);
    }
    
    public async Task<UserProfileData?> GetProfileAsync(
        Guid userId, CancellationToken ct = default)
    {
        var profile = await _repository.GetByUserIdAsync(userId, ct);
        if (profile is null) return null;

        return new UserProfileData(
            profile.FirstName,
            profile.LastName,
            profile.FullName,
            profile.BirthDate);
    }
    
    public async Task<string?> GetFullNameAsync(
        Guid userId, CancellationToken ct = default)
    {
        var profile = await _repository.GetByUserIdAsync(userId, ct);
        return profile?.FullName;
    }
    
    public async Task AnonymiseProfileAsync(
        Guid userId, 
        string deletedBy, 
        CancellationToken ct = default)
    {
        var profile = await _repository.GetByUserIdAsync(userId, ct);
        if (profile is null) return;

        profile.Anonymise(deletedBy);
        
        await _repository.SaveChangesAsync(ct);
    }
    
    public async Task<Result<UserProfileData>> UpdateProfileAsync(
        Guid userId,
        string firstName,
        string lastName,
        DateOnly birthDate,
        CancellationToken ct = default)
    {
        var profile = await _repository.GetByUserIdAsync(userId, ct);
        if (profile is null)
        {
            UserLogMessages.UpdateProfileUserNotFound(_logger, userId, null);
            return Result<UserProfileData>.NotFound(UserErrors.UserNotFound);
        }

        profile.UpdateProfile(firstName, lastName, birthDate, userId.ToString());
        await _repository.SaveChangesAsync(ct);

        UserLogMessages.UpdateProfileSucceeded(_logger, userId, null);

        return Result<UserProfileData>.Success(new UserProfileData(
            profile.FirstName,
            profile.LastName,
            profile.FullName,
            profile.BirthDate));
    }
}