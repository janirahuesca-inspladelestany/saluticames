using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Responses;
using Application.Challenge.Services;
using Contracts.DTO.Challenge;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]"), Authorize]
[ApiController]
public class HikerController(IChallengeService _challengeService) : ControllerBase
{
    /// <summary>
    /// Crea un nou excursionista
    /// </summary>
    /// <param name="createHikerRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna un codi de resposta segons el resultat</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Recupera les estadístiques de l'excursionista especificat
    /// </summary>
    /// <param name="id"></param>
    /// <param name="catalogueId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna un codi de resposta segons el resultat</returns>
    [HttpGet("{id}/stats")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveStatisticsAsync(string id, [FromQuery] Guid? catalogueId = null, CancellationToken cancellationToken = default)
    {
        // Cridar servei d'aplicació
        var getStatisticsResult = await _challengeService.GetStatisticsAsync(id, catalogueId, cancellationToken: cancellationToken);

        // Retornar Model/Resposta o error
        return getStatisticsResult.Match(
            result =>
            {
                var retieveStatisticsResponse = result!.ToDictionary(kv => kv.Key, kv =>
                    new RetrieveStatisticsResponse(
                        ReachedSummits: kv.Value.ReachedSummits,
                        PendingSummits: kv.Value.PendingSummits));

                return retieveStatisticsResponse.Any() ? Ok(retieveStatisticsResponse) : NoContent();
            },
            error => error.ToProblemDetails());
    }

    /// <summary>
    /// Registra noves ascensions per a l'excursionista especificat
    /// </summary>
    /// <param name="id"></param>
    /// <param name="createClimbRequests"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna un codi de resposta segons el resultat</returns>
    [HttpPost("{id}/climbs")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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
            result => result!.Any() ? Created(string.Empty, result) : NoContent(),
            error => error.ToProblemDetails());
    }

    /// <summary>
    /// Recupera les ascensions registrades per a l'excursionista especificat
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna un codi de resposta segons el resultat</returns>
    [HttpGet("{id}/climbs")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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


    /// <summary>
    /// Crea un nou diari per a l'excursionista especificat
    /// </summary>
    /// <param name="id"></param>
    /// <param name="createDiaryRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna un codi de resposta segons el resultat</returns>
    [HttpPost("{id}/diary")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Recupera els diaris de l'excursionista especificat
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}/diary")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
