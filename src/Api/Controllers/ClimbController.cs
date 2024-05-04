using Api.Extensions;
using Api.Models.Requests;
using Application.ChallengeContext.Services;
using Contracts.DTO.Catalogue;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ClimbController(IChallengeService _challengeService) : ControllerBase
{
    [HttpPost("hiker/{hikerId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateClimbsAsync(Guid hikerId, IEnumerable<CreateClimbRequest> createClimbRequests, CancellationToken cancellationToken = default)
    {
        var climbsToCreate = createClimbRequests
            .ToList()
            .ConvertAll(climb =>
                new CreateClimbDetailDto(
                    SummitId: climb.summitId, 
                    AscensionDateTime: climb.ascensionDateTime));

        var result = await _challengeService.CreateClimbsAsync(hikerId, climbsToCreate, cancellationToken);

        return result.Match(
            result => Ok(result),
            error => result.ToProblemDetails());
    }
}
