namespace MedCorVis.Modules.Users.Application.Abstractions;

using MedCorVis.Common.Results;
using MedCorVis.Modules.Users.Application.Contracts.Requests;
using MedCorVis.Modules.Users.Application.Contracts.Responses;

public interface IUserService
{
    Task<Result<UserResponse>> GetCurrentUserAsync(Guid userId, CancellationToken ct = default);
    Task<Result<bool>> UpdateCultureAsync(Guid userId, string culture, CancellationToken ct = default);
    Task<Result<UserResponse>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default);
    Task<Result<bool>> UpdatePhoneAsync(Guid userId, string? phoneNumber, CancellationToken ct = default);
}