using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;

namespace Api.Extensions;

public static class ResultExtensions
{
    /// <summary>
    /// Mètode d'extensió per convertir un error en una resposta HTTP amb ProblemDetails
    /// </summary>
    /// <typeparam name="TError"></typeparam>
    /// <param name="error"></param>
    /// <returns>// Retorna una resposta HTTP amb el ProblemDetails</returns>
    public static IActionResult ToProblemDetails<TError>(this TError error)
        where TError : Error
    {
        // Crear un objecte ProblemDetails per descriure l'error
        var problem = new ProblemDetails()
        {
            Status = GetStatusCode(error.Type), // Assignar el codi d'estat HTTP corresponent
            Type = GetType(error.Type), // Assignar el tipus de problema en format URL
            Title = error.Type.ToString(), // Assignar el tipus d'error com a títol
            Extensions = new Dictionary<string, object?>() // Afegir detalls addicionals de l'error
            {
                [nameof(error)] = error
            }
        };

        return new ObjectResult(problem);

        // Funció local per obtenir el codi d'estat HTTP basat en el tipus d'error
        static int GetStatusCode(Error.ErrorType errorType) =>
            errorType switch
            {
                Error.ErrorType.Validation => StatusCodes.Status400BadRequest, // Error de validació
                Error.ErrorType.NotFound => StatusCodes.Status404NotFound, // Error de no trobat
                Error.ErrorType.Conflict => StatusCodes.Status409Conflict, // Error de conflicte
                _ => StatusCodes.Status500InternalServerError // Error intern del servidor
            };

        // Funció local per obtenir el tipus de problema en format URL basat en el tipus d'error
        static string GetType(Error.ErrorType errorType) =>
            errorType switch
            {
                Error.ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1", // URL per error de validació
                Error.ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4", // URL per error de no trobat
                Error.ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8", // URL per error de conflicte
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1" // URL per error intern del servidor
            };
    }
}
