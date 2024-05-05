using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Responses;
using Application.ChallengeContext.Services;
using Contracts.DTO.Challenge;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class DiaryController(IChallengeService _challengeService) : ControllerBase
{
    [HttpPost("hiker/{hikerId}/catalogue/{catalogueId}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateDiaryAsync(string hikerId, Guid catalogueId, CreateDiaryRequest createDiaryRequest, CancellationToken cancellationToken = default)
    {
        // Mapejar Model/Request a Contract/DTO
        var diaryToCreate = new CreateDiaryDetailDto(
            Name: createDiaryRequest.Name,
            HikerId: hikerId,
            CatalogueId: catalogueId);

        // Cridar servei d'aplicació
        var createDiaryResult = await _challengeService.CreateDiaryAsync(diaryToCreate, cancellationToken);

        // Retornar Model/Resposta o error
        return createDiaryResult.Match(
            result => Ok(result),
            error => error.ToProblemDetails());
    }

    [HttpGet("hiker/{hikerId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReadDiaryAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        // Mapejar Model/Request a Contract/DTO
        var filter = new GetDiariesFilterDto(HikerId: hikerId);

        // Cridar servei d'aplicació
        var getDiaryResult = await _challengeService.GetDiariesAsync(filter, cancellationToken);

        return getDiaryResult.Match(
            result =>
            {
                var readDiaryResponse = result!.ToDictionary(kv => kv.Key, kv =>
                    new ReadDiaryResponse(
                        Name: kv.Value.Name));

                return readDiaryResponse.Any() ? Ok(readDiaryResponse) : NoContent();
            },
            error => error.ToProblemDetails());
    }
}
