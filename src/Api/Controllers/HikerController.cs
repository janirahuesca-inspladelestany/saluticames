using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Responses;
using Application.ChallengeContext.Services;
using Contracts.DTO.Challenge;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class HikerController(IChallengeService _challengeService) : ControllerBase
{
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateHikerAsync(CreateHikerRequest createHikerRequest, CancellationToken cancellationToken = default)
    {
        // Mapejar Model/Request a Contract/DTO
        var hikerToCreate = new CreateHikerDetailDto(
            Id: createHikerRequest.Id,
            Name: createHikerRequest.Name,
            Surname: createHikerRequest.Surname);

        // Cridar servei d'aplicació
        var createHikerResult = await _challengeService.CreateHikerAsync(hikerToCreate, cancellationToken);

        // Retornar Model/Resposta o error
        return createHikerResult.Match(
            () => Created(),
            error => error.ToProblemDetails());
    }

    [HttpGet("{id}/stats")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReadStatsAsync(string id, [FromQuery] Guid? catalogueId = null, CancellationToken cancellationToken = default)
    {
        // Cridar servei d'aplicació
        var getStatsResult = await _challengeService.GetStatisticsAsync(id, catalogueId, cancellationToken: cancellationToken);

        // Retornar Model/Resposta o error
        return getStatsResult.Match(
            result =>
            {
                var readStatsResponse = result!.ToDictionary(kv => kv.Key, kv => 
                    new ReadStatisticsResponse(
                        ReachedSummits: kv.Value.ReachedSummits, 
                        PendingSummits: kv.Value.PendingSummits));

                return readStatsResponse.Any() ? Ok(readStatsResponse) : NoContent();
            },
            error => error.ToProblemDetails());
    }
}
