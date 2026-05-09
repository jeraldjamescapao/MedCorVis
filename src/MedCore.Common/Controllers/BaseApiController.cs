namespace MedCore.Common.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MedCore.Common.ProblemDetails;
using MedCore.Common.Results;
using MedCore.Common.Services;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult ToActionResult<T>(Result<T> result, int successStatusCode = StatusCodes.Status200OK)
    {
        if (result.IsSuccess)
            return StatusCode(successStatusCode, result.Value);

        var problem = ProblemDetailsHelper.FromResult(
            result.Error!,
            result.ErrorType,
            Request.Path,
            HttpContext.TraceIdentifier);

        return StatusCode(problem.Status!.Value, problem);
    }

    protected static bool TryGetCurrentUserId(ICurrentUserService currentUserService, out Guid userId)
    {
        return Guid.TryParse(currentUserService.UserId, out userId);
    }
}