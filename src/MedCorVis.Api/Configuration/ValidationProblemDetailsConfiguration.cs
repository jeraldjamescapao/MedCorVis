namespace MedCorVis.Api.Configuration;

using MedCorVis.Common.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

internal static class ValidationProblemDetailsConfiguration
{
    internal static IServiceCollection AddValidationProblemDetails(
        this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value!.Errors.Select(x => x.ErrorMessage).ToArray());

                var problem = ProblemDetailsHelper.ForInvalidRequest(
                    errors,
                    context.HttpContext.Request.Path,
                    context.HttpContext.TraceIdentifier);

                return new ObjectResult(problem)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            };
        });

        return services;
    }
}