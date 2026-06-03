namespace MedCorVis.Modules.Users.Presentation.Controllers;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedCorVis.Common.Controllers;
using MedCorVis.Common.Results;
using MedCorVis.Common.Services;
using MedCorVis.Common.UserProfiles;
using MedCorVis.Modules.Users.Application.Contracts.Requests;
using MedCorVis.Modules.Users.Application.Errors;

[Authorize]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/users")]
public sealed class UsersConsumerController : BaseApiController
{
    private readonly IUserProfileService _userProfileService;
    private readonly ICurrentUserService _currentUserService;
    
    public UsersConsumerController(
        IUserProfileService userService, 
        ICurrentUserService currentUserService)
    {
        _userProfileService = userService;
        _currentUserService = currentUserService;
    }
    
    [HttpPut("me/profile")]
    public async Task<IActionResult> UpdateProfileAsync(
        [FromBody] UpdateProfileRequest request, CancellationToken ct)
    {
        if (!TryGetCurrentUserId(_currentUserService, out var userId))
            return ToActionResult(Result<UserProfileData>.Unauthorized(UserErrors.InvalidToken));

        var result = await _userProfileService.UpdateProfileAsync(
            userId, 
            request.FirstName, 
            request.LastName, 
            request.BirthDate, 
            ct);

        return ToActionResult(result);
    }
}