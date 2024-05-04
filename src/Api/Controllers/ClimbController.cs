using Api.Extensions;
using Api.Models.Requests;
using Application.ChallengeContext.Services;
using Contracts.DTO.Challenge;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ClimbController(IChallengeService _challengeService) : ControllerBase
{
    [HttpPost("hiker/{hikerId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateClimbsAsync(string hikerId, IEnumerable<CreateClimbRequest> createClimbRequests, CancellationToken cancellationToken = default)
    {
        var climbsToCreate = createClimbRequests
            .ToList()
            .ConvertAll(climb =>
                new CreateClimbDetailDto(
                    SummitId: climb.summitId, 
                    AscensionDateTime: climb.ascensionDateTime));

        var createClimbsResult = await _challengeService.CreateClimbsAsync(hikerId, climbsToCreate, cancellationToken);

        return createClimbsResult.Match(
            result => Ok(result),
            error => error.ToProblemDetails());
    }

    [HttpGet("hiker/{hikerId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReadClimbsAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        var getClimbsResult = await _challengeService.GetClimbsAsync(hikerId, cancellationToken);

        return getClimbsResult.Match(
            result => Ok(result),
            error => error.ToProblemDetails());
    }
}
