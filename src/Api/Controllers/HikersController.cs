using Api.Extensions;
using Api.Models.Requests.Queries;
using Api.Models.Responses;
using Application.Challenge.Services;
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
