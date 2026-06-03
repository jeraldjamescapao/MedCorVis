namespace MedCorVis.Common.Account;

using MedCorVis.Common.Results;

public interface IAccountService
{
    Task<Result<AccountResponse>> GetAccountAsync(Guid userId, CancellationToken ct = default);
    Task<Result<bool>> UpdateCultureAsync(Guid userId, string culture, CancellationToken ct = default);
    Task<Result<bool>> UpdatePhoneAsync(Guid userId, string? phoneNumber, CancellationToken ct = default);
    Task<Result<bool>> RequestDeletionAsync(Guid userId, CancellationToken ct = default);
    Task<Result<bool>> CancelDeletionRequestAsync(Guid userId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<DeletionRequestResponse>>> GetPendingDeletionRequestsAsync(CancellationToken ct = default);
    Task<Result<bool>> ExecuteDeletionAsync(Guid actorId, Guid targetUserId, CancellationToken ct = default);
}