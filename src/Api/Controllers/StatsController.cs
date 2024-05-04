using Api.Models.Responses;
using Application.ChallengeContext.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class StatsController(IChallengeService _challengeService) : ControllerBase
{
    [HttpGet("user/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatsAsync(Guid id)
    {
        var stats = await _challengeService.GetStatisticsAsync(id);

        stats.ToDictionary(kv => kv.Key, kv => new GetStatisticsResponse(kv.Value.ReachedSummits, kv.Value.PendingSummits));

        return new OkObjectResult(stats);
    }

    [HttpGet("user/{id:guid}/catalogue/{catalogueId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatsByCatalogueIdAsync(Guid id, Guid catalogueId)
    {
        var stats = await _challengeService.GetStatisticsAsync(id, catalogueId);

        var response = new GetStatisticsResponse(stats.ReachedSummits, stats.PendingSummits);

        return new OkObjectResult(stats);
    }
}
