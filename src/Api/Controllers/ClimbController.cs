using Api.Extensions;
using Api.Models.Requests;
using Application.ChallengeContext.Services;
using Contracts.DTO.Challenge;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ClimbController(IChallengeService _challengeService) : ControllerBase
{
    [HttpPost("hiker/{hikerId}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateClimbsAsync(string hikerId, IEnumerable<CreateClimbRequest> createClimbRequests, CancellationToken cancellationToken = default)
    {
        // Mapejar Model/Request a Contract/DTO
        var climbsToCreate = createClimbRequests
            .ToList()
            .ConvertAll(climb =>
                new CreateClimbDetailDto(
                    SummitId: climb.summitId, 
                    AscensionDateTime: climb.ascensionDateTime));
        
        // Cridar servei d'aplicació
        var createClimbsResult = await _challengeService.CreateClimbsAsync(hikerId, climbsToCreate, cancellationToken);

        // Retornar Model/Resposta o error
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
        // Cridar servei d'aplicació
        var getClimbsResult = await _challengeService.GetClimbsAsync(hikerId, cancellationToken);

        // Retornar Model/Resposta o error
        return getClimbsResult.Match(
            result =>
            {
                var readClimbsResponse = result!.ToDictionary(kv => kv.Key, kv =>
                    new ReadClimbResponse(
                        SummitId: kv.Value.SummitId,
                        AscensionDate: kv.Value.AscensionDateTime));

                return readClimbsResponse.Any() ? Ok(readClimbsResponse) : NoContent();
            },
            error => error.ToProblemDetails());
    }
}
