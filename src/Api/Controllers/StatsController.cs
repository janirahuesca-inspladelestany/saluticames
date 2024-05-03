using Api.Models.Responses;
using Application.ChallengeContext.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class StatsController(IChallengeService _challengeService) : ControllerBase
{
    [HttpGet("user/{id:guid}")]
    public async Task<IActionResult> GetStatsAsync(Guid id)
    {
        var stats = await _challengeService.GetStatisticsAsync(id);

        stats.ToDictionary(kv => kv.Key, kv => new GetStatisticsResponse(kv.Value.ReachedSummits, kv.Value.PendingSummits));

        return new OkObjectResult(stats);
    }
}
