using SharedKernel.Common;

namespace Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails<TValue, TError>(this Result<TValue, TError> result)
        where TError : Error
    {
        if (result.IsSuccess()) throw new InvalidOperationException();

        return Results.Problem(
            statusCode: GetStatusCode(result.Error.Type),
            title: result.Error.Type.ToString(),
            type: GetType(result.Error.Type),
            extensions: new Dictionary<string, object?>
            {
                { "error", result.Error }
            });

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
