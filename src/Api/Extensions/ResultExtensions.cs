using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;

namespace Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToProblemDetails<TError>(this TError error)
        where TError : Error
    {
        var problem = new ProblemDetails()
        {
            Status = GetStatusCode(error.Type),
            Type = GetType(error.Type),
            Title = error.Type.ToString(),
            Extensions = new Dictionary<string, object?>()
            {
                [nameof(error)] = error
            }
        };

        return new ObjectResult(problem);

        static int GetStatusCode(Error.ErrorType errorType) =>
            errorType switch
            {
                Error.ErrorType.Validation => StatusCodes.Status400BadRequest,
                Error.ErrorType.NotFound => StatusCodes.Status404NotFound,
                Error.ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

        static string GetType(Error.ErrorType errorType) =>
            errorType switch
            {
                Error.ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Error.ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Error.ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
    }
}
