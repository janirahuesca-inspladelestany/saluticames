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
    public class HikersController(IChallengeService _challengeService) : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ReadHikersAsync([FromQuery] ReadHikersQuery readHikersQuery, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var filter = new GetHikersFilterDto(readHikersQuery.Id, readHikersQuery.Name, readHikersQuery.Surname);

            // Cridar servei d'aplicació
            var getHikersResult = await _challengeService.GetHikersAsync(filter, cancellationToken);

            // Retornar Model/Resposta o error
            return getHikersResult.Match(
                result =>
                {
                    var readHikersResponse = result!.ToDictionary(kv => kv.Key, kv =>
                        new ReadHikersResponse(
                            Name: kv.Value.Name,
                            Surname: kv.Value.Surname));

                    return readHikersResponse.Any() ? Ok(readHikersResponse) : NoContent();
                },
                error => error.ToProblemDetails());
        }
    }
}
