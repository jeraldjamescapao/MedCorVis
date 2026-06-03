namespace MedCorVis.Modules.Users.Application.Abstractions;

using MedCorVis.Common.Results;
using MedCorVis.Modules.Users.Application.Contracts.Requests;
using MedCorVis.Modules.Users.Application.Contracts.Responses;

public interface IUserService
{
    Task<Result<UserProfileResponse>> UpdateProfileAsync(
        Guid userId, UpdateProfileRequest request, CancellationToken ct = default);
}