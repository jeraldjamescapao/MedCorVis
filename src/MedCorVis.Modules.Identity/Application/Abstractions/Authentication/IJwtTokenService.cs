namespace MedCorVis.Modules.Identity.Application.Abstractions.Authentication;

using MedCorVis.Modules.Identity.Domain.Users;

internal interface IJwtTokenService
{
    string GenerateAccessToken(ApplicationUser user, IList<string> roles);
    string GenerateRefreshToken();
}