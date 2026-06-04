namespace MedCorVis.Modules.Identity.Application.Abstractions.Email;

using MedCorVis.Modules.Identity.Domain.Users;

internal interface IIdentityEmailService
{
    Task SendConfirmationEmailAsync(
        Guid userId,
        string email,
        string fullName,
        string encodedToken,
        string culture,
        CancellationToken ct = default);
    
    Task SendPasswordResetEmailAsync(
        Guid userId,
        string email,
        string fullName,
        string encodedToken,
        string culture,
        CancellationToken ct = default);
}