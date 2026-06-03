namespace MedCorVis.Common.Account;

public sealed record DeletionRequestResponse(
    Guid UserId,
    string FullName,
    string Email,
    DateTimeOffset DeletionRequestedAtUtc);