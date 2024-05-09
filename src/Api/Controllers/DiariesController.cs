using Api.Extensions;
using Api.Models.Requests.Queries;
using Api.Models.Responses;
using Application.ChallengeContext.Services;
using Contracts.DTO.Challenge;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DiariesController(IChallengeService _challengeService) : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ReadDiariesAsync([FromQuery] ReadDiariesQuery readDiariesQuery, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var filter = new GetDiariesFilterDto(readDiariesQuery.Id, readDiariesQuery.Name, readDiariesQuery.HikerId);

            // Cridar servei d'aplicació
            var getDiariesResult = await _challengeService.GetDiariesAsync(filter, cancellationToken);

            // Retornar Model/Resposta o error
            return getDiariesResult.Match(
                result =>
                {
                    var readDiariesResponse = result!.ToDictionary(kv => kv.Key, kv =>
                        kv.Value.Select(value => new ReadDiariesResponse(
                            Id: value.Id,
                            Name: value.Name)));

                    return readDiariesResponse.Any() ? Ok(readDiariesResponse) : NoContent();
                },
                error => error.ToProblemDetails());
        }
    }
}
