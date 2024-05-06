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
            () => Created(string.Empty, createHikerRequest.Id),
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

    [HttpPost("{id}/climbs")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateClimbsAsync(string id, IEnumerable<CreateClimbRequest> createClimbRequests, CancellationToken cancellationToken = default)
    {
        // Mapejar Model/Request a Contract/DTO
        var climbsToCreate = createClimbRequests
            .ToList()
            .ConvertAll(climb =>
                new CreateClimbDetailDto(
                    SummitId: climb.summitId,
                    AscensionDateTime: climb.ascensionDateTime));

        // Cridar servei d'aplicació
        var createClimbsResult = await _challengeService.CreateClimbsAsync(id, climbsToCreate, cancellationToken);

        // Retornar Model/Resposta o error
        return createClimbsResult.Match(
            result => Created(string.Empty, result),
            error => error.ToProblemDetails());
    }

    [HttpGet("{id}/climbs")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReadClimbsAsync(string id, CancellationToken cancellationToken = default)
    {
        // Cridar servei d'aplicació
        var getClimbsResult = await _challengeService.GetClimbsAsync(id, cancellationToken);

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

    [HttpPost("{id}/diary")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateDiaryAsync(string id, CreateDiaryRequest createDiaryRequest, CancellationToken cancellationToken = default)
    {
        // Mapejar Model/Request a Contract/DTO
        var diaryToCreate = new CreateDiaryDetailDto(
            Name: createDiaryRequest.Name,
            HikerId: id,
            CatalogueId: createDiaryRequest.CatalogueId);

        // Cridar servei d'aplicació
        var createDiaryResult = await _challengeService.CreateDiaryAsync(diaryToCreate, cancellationToken);

        // Retornar Model/Resposta o error
        return createDiaryResult.Match(
            result => Created(string.Empty, result),
            error => error.ToProblemDetails());
    }

    [HttpGet("{id}/diary")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReadDiaryAsync(string id, CancellationToken cancellationToken = default)
    {
        // Mapejar Model/Request a Contract/DTO
        var filter = new GetDiariesFilterDto(HikerId: id);

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
