namespace MedCorVis.Modules.Identity.Application.Abstractions.Authentication;

using MedCorVis.Common.Results;
using MedCorVis.Modules.Identity.Application.Contracts.Authentication.Requests;
using MedCorVis.Modules.Identity.Application.Contracts.Authentication.Responses;

public interface IAuthService
{
    Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<Result<RefreshResponse>> RefreshAsync(string refreshToken, CancellationToken ct = default);
    Task<Result<bool>> LogoutAsync(string refreshToken, CancellationToken ct = default);
    Task<Result<bool>> LogoutAllAsync(Guid userId, CancellationToken ct = default);
    Task<Result<bool>> ConfirmEmailAsync(Guid userId, string token, CancellationToken ct = default);
    Task<Result<bool>> ResendConfirmationEmailAsync(string email, CancellationToken ct = default);
}