using Api.Extensions;
using Api.Models.Requests.Queries;
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
            var filter = new GetHikersFilterDto(readHikersQuery.Id, readHikersQuery.Name, readHikersQuery.Surname);
            var getHikersResult = await _challengeService.GetHikersAsync(filter, cancellationToken);

            return getHikersResult.Match(
                result => Ok(result),
                error => error.ToProblemDetails());
        }
    }
}
