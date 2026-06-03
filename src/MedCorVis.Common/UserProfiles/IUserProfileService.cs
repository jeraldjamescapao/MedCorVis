namespace MedCorVis.Common.UserProfiles;

public interface IUserProfileService
{
    Task CreateProfileAsync(
        Guid userId,
        string firstName,
        string lastName,
        DateOnly birthDate,
        string createdBy,
        CancellationToken ct = default);
    
    Task<UserProfileData?> GetProfileAsync(Guid userId, CancellationToken ct = default);
    Task<string?> GetFullNameAsync(Guid userId, CancellationToken ct = default);
    Task AnonymiseProfileAsync(Guid userId, string deletedBy, CancellationToken ct = default);
}