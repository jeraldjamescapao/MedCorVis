namespace MedCorVis.Modules.Users.Application.Services;

using MedCorVis.Common.UserProfiles;
using MedCorVis.Modules.Users.Application.Abstractions;
using MedCorVis.Modules.Users.Domain;

internal sealed class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _repository;

    public UserProfileService(IUserProfileRepository repository)
    {
        _repository = repository;
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
}