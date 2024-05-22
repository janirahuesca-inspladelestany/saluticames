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
    [Authorize]
    public class DiariesController(IChallengeService _challengeService) : ControllerBase
    {
        /// <summary>
        ///  Retorna una llista de diaris basats en els criteris de filtre especificats a RetrieveDiariesQuery
        /// </summary>
        /// <param name="retieveDiariesQuery"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Retorna un codi de resposta segons el resultat</returns>
        [HttpGet]
        [Produces("application/json"), Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrieveDiariesAsync([FromQuery] RetrieveDiariesQuery retieveDiariesQuery, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var filterDto = new ListDiariesFilterDto(retieveDiariesQuery.Id, retieveDiariesQuery.Name, retieveDiariesQuery.HikerId);

            // Cridar servei d'aplicació
            var listDiariesResult = await _challengeService.ListDiariesAsync(filterDto, cancellationToken);

            // Retornar Model/Resposta o error
            return listDiariesResult.Match(
                result =>
                {
                    var retrieveDiariesResponse = result!.ToDictionary(kv => kv.Key, kv =>
                        kv.Value.Select(value => new RetrieveDiariesResponse(
                            Id: value.Id,
                            Name: value.Name)));

                    return retrieveDiariesResponse.Any() ? Ok(retrieveDiariesResponse) : NoContent();
                },
                error => error.ToProblemDetails());
        }
    }
}
