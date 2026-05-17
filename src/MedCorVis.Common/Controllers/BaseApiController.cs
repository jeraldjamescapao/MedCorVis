namespace MedCorVis.Common.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MedCorVis.Common.ProblemDetails;
using MedCorVis.Common.Results;
using MedCorVis.Common.Services;

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
    
    protected string GetVersionedPath(string suffix)
    {
        var version = HttpContext.GetRequestedApiVersion()?.MajorVersion;
        return $"/api/v{version}/{suffix}";
    }
}