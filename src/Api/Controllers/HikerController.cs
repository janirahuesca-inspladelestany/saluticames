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
    public async Task<IActionResult> CreateHikerAsync(CreateHikerRequest createHikerRequest, CancellationToken cancellationToken = default)
    {
        var hikerToCreate = new CreateHikerDetailDto(
            Id: createHikerRequest.Id,
            Name: createHikerRequest.Name,
            Surname: createHikerRequest.Surname);

        var createHikerResult = await _challengeService.CreateHikerAsync(hikerToCreate, cancellationToken);

        return createHikerResult.Match(
            () => Ok(),
            error => error.ToProblemDetails());
    }

    [HttpGet("{id}/stats")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReadStatsAsync(string id, [FromQuery] Guid? catalogueId = null, CancellationToken cancellationToken = default)
    {
        var getStatsResult = await _challengeService.GetStatisticsAsync(id, catalogueId, cancellationToken: cancellationToken);

        return getStatsResult.Match(
            value => Ok(value!.ToDictionary(kv => kv.Key, kv => new GetStatisticsResponse(kv.Value.ReachedSummits, kv.Value.PendingSummits))),
            error => error.ToProblemDetails());
    }
}
