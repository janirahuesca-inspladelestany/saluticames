using Api.Extensions;
using Api.Models.Requests.Queries;
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
            var filter = new GetDiariesFilterDto(readDiariesQuery.Id, readDiariesQuery.Name, readDiariesQuery.HikerId);
            var getDiariesResult = await _challengeService.GetDiariesAsync(filter, cancellationToken);

            return getDiariesResult.Match(
                result => Ok(result),
                error => error.ToProblemDetails());
        }
    }
}
