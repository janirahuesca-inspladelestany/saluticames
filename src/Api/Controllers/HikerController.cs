using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Responses;
using Application.Challenge.Services;
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
        var hikerDto = new AddNewHikerDto(
            Id: createHikerRequest.Id,
            Name: createHikerRequest.Name,
            Surname: createHikerRequest.Surname);

        // Cridar servei d'aplicació
        var addNewHikerResult = await _challengeService.AddNewHikerAsync(hikerDto, cancellationToken);

        // Retornar Model/Resposta o error
        return addNewHikerResult.Match(
            () => Created(string.Empty, createHikerRequest.Id),
            error => error.ToProblemDetails());
    }

    [HttpGet("{id}/stats")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RetrieveStatisticsAsync(string id, [FromQuery] Guid? catalogueId = null, CancellationToken cancellationToken = default)
    {
        // Cridar servei d'aplicació
        var getStatisticsResult = await _challengeService.GetStatisticsAsync(id, catalogueId, cancellationToken: cancellationToken);

        // Retornar Model/Resposta o error
        return getStatisticsResult.Match(
            result =>
            {
                var retieveStatisticsResponse = result!.ToDictionary(kv => kv.Key, kv =>
                    new RetieveStatisticsResponse(
                        ReachedSummits: kv.Value.ReachedSummits,
                        PendingSummits: kv.Value.PendingSummits));

                return retieveStatisticsResponse.Any() ? Ok(retieveStatisticsResponse) : NoContent();
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
        var climbDtos = createClimbRequests
            .ToList()
            .ConvertAll(climb =>
                new AddNewClimbDetailDto(
                    SummitId: climb.summitId,
                    AscensionDateTime: climb.ascensionDateTime));

        // Cridar servei d'aplicació
        var addNewClimbsResult = await _challengeService.AddNewClimbsAsync(id, climbDtos, cancellationToken);

        // Retornar Model/Resposta o error
        return addNewClimbsResult.Match(
            result => Created(string.Empty, result),
            error => error.ToProblemDetails());
    }

    [HttpGet("{id}/climbs")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RetrieveClimbsAsync(string id, CancellationToken cancellationToken = default)
    {
        // Cridar servei d'aplicació
        var findClimbsResult = await _challengeService.FindClimbsAsync(id, cancellationToken);

        // Retornar Model/Resposta o error
        return findClimbsResult.Match(
            result =>
            {
                var retrieveClimbsResponse = result!.ToDictionary(kv => kv.Key, kv =>
                    new RetrieveClimbResponse(
                        SummitId: kv.Value.SummitId,
                        AscensionDate: kv.Value.AscensionDateTime));

                return retrieveClimbsResponse.Any() ? Ok(retrieveClimbsResponse) : NoContent();
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
        var diaryDto = new AddNewDiaryDto(
            Name: createDiaryRequest.Name,
            HikerId: id,
            CatalogueId: createDiaryRequest.CatalogueId);

        // Cridar servei d'aplicació
        var addNewDiaryResult = await _challengeService.AddNewDiaryAsync(diaryDto, cancellationToken);

        // Retornar Model/Resposta o error
        return addNewDiaryResult.Match(
            result => Created(string.Empty, result),
            error => error.ToProblemDetails());
    }

    [HttpGet("{id}/diary")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RetrieveDiaryAsync(string id, CancellationToken cancellationToken = default)
    {
        // Mapejar Model/Request a Contract/DTO
        var filterDto = new ListDiariesFilterDto(HikerId: id);

        // Cridar servei d'aplicació
        var listDiariesResult = await _challengeService.ListDiariesAsync(filterDto, cancellationToken);

        // Retornar Model/Resposta o error
        return listDiariesResult.Match(
            result =>
            {
                var retrieveDiaryResponse = result!.ToDictionary(kv => kv.Key, kv => kv.Value.Select(
                    value => new RetrieveDiaryResponse(Id: value.Id, Name: value.Name)));

                return retrieveDiaryResponse.Any() ? Ok(retrieveDiaryResponse) : NoContent();
            },
            error => error.ToProblemDetails());
    }
}
