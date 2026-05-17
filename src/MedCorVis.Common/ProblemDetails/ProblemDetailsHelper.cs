namespace MedCorVis.Common.ProblemDetails;

using MedCorVis.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public static class ProblemDetailsHelper
{
    private static readonly IReadOnlyDictionary<ResultErrorType, (int StatusCode, string Type, string Title)> Map =
        new Dictionary<ResultErrorType, (int, string, string)>
        {
            [ResultErrorType.Validation] = (
                400,
                RfcLinks.Status400,
                "Bad Request"),
            [ResultErrorType.Unauthorized] = (
                401,
                RfcLinks.Status401,
                "Unauthorized"),
            [ResultErrorType.Forbidden] = (
                403,
                RfcLinks.Status403,
                "Forbidden"),
            [ResultErrorType.NotFound] = (
                404,
                RfcLinks.Status404,
                "Not Found"),
            [ResultErrorType.Conflict] = (
                409,
                RfcLinks.Status409,
                "Conflict"),
            [ResultErrorType.UnprocessableEntity] = (
                422,
                RfcLinks.Status422,
                "Unprocessable Content"),
            [ResultErrorType.Internal] = (
                500,
                RfcLinks.Status500,
                "Internal Server Error"),
            [ResultErrorType.ServiceUnavailable] = (
                503,
                RfcLinks.Status503,
                "Service Unavailable"),
        };
    
    public static ProblemDetails FromResult(
        ResultError error, 
        ResultErrorType errorType, 
        string instance, 
        string traceId)
    {
        var entry = GetEntry(errorType);
        
        return new ProblemDetails
        {
            Type = entry.Type,
            Title = entry.Title,
            Status = entry.StatusCode,
            Detail = error.Message,
            Instance = instance,
            Extensions =
            {
                ["traceId"] = traceId,
                ["code"] = error.Code
            }
        };
    }
    
    public static ProblemDetails ForInvalidRequest(
        Dictionary<string, string[]> errors,
        string instance,
        string traceId)
    {
        return new ProblemDetails
        {
            Type     = RfcLinks.Status400,
            Title    = "Bad Request",
            Status   = StatusCodes.Status400BadRequest,
            Detail   = "One or more validation errors occurred.",
            Instance = instance,
            Extensions =
            {
                ["traceId"] = traceId,
                ["errors"]  = errors
            }
        };
    }
    
    private static (int StatusCode, string Type, string Title) GetEntry(ResultErrorType errorType)
    {
        if (!Map.TryGetValue(errorType, out var entry))
            throw new InvalidOperationException($"Unhandled error type: {errorType}.");

        return entry;
    }
}