using Api.Extensions;
using Api.Models.Requests.Queries;
using Api.Models.Responses;
using Application.Challenge.Services;
using Contracts.DTO.Challenge;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]"), Authorize]
    [ApiController]
    public class HikersController(IChallengeService _challengeService) : ControllerBase
    {
        /// <summary>
        /// Recupera una llista d'excursionistes segons els filtres proporcionats a la consulta
        /// </summary>
        /// <param name="retrieveHikersQuery"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Retorna un codi de resposta segons el resultat</returns>
        [HttpGet, Authorize(Roles = "Admin")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrieveHikersAsync([FromQuery] RetrieveHikersQuery retrieveHikersQuery, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var filterDto = new ListHikersFilterDto(retrieveHikersQuery.Id, retrieveHikersQuery.Name, retrieveHikersQuery.Surname);

            // Cridar servei d'aplicació
            var listHikersResult = await _challengeService.ListHikersAsync(filterDto, cancellationToken);

            // Retornar Model/Resposta o error
            return listHikersResult.Match(
                result =>
                {
                    var retrieveHikersResponse = result!.ToDictionary(kv => kv.Key, kv =>
                        new RetrieveHikersResponse(
                            Name: kv.Value.Name,
                            Surname: kv.Value.Surname));

                    return retrieveHikersResponse.Any() ? Ok(retrieveHikersResponse) : NoContent();
                },
                error => error.ToProblemDetails());
        }
    }
}
