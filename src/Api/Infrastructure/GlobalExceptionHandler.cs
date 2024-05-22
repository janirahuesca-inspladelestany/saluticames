using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Infrastructure;

// Implementació del gestor global d'excepcions
internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    /// <summary>
    /// Mètode que intenta gestionar les excepcions
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="exception"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>// Retorna true per indicar que l'excepció ha estat gestionada</returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Registra l'excepció no gestionada amb el nivell de registre d'error
        logger.LogError(exception, "Unhandled exception ocurred.");

        // Crea un objecte ProblemDetails per representar la resposta d'error
        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError, // // Estableix el codi d'estat HTTP a 500
            Type = "http://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1", // // URL a la documentació del codi
            Title = "Server failure" // Títol que descriu el problema
        };

        // Estableix el codi d'estat HTTP de la resposta
        httpContext.Response.StatusCode = problemDetails.Status.Value;

        // Escriu l'objecte ProblemDetails com a resposta JSON
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
