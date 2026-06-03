namespace MedCorVis.Modules.Users.Application.Contracts.Responses;

public sealed record UserProfileResponse(
    string FirstName,
    string LastName,
    string FullName,
    DateOnly BirthDate);