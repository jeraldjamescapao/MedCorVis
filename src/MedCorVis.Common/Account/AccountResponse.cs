namespace MedCorVis.Common.Account;

public sealed record AccountResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    DateOnly BirthDate,
    string? Culture,
    string? PhoneNumber,
    bool IsActive,
    bool IsDeleted,
    DateTimeOffset? DeletionRequestedAtUtc,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? ModifiedAtUtc);