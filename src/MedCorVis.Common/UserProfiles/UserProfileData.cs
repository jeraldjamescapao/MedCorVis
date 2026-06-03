namespace MedCorVis.Common.UserProfiles;

public sealed record UserProfileData(
    string FirstName,
    string LastName,
    string FullName,
    DateOnly BirthDate);