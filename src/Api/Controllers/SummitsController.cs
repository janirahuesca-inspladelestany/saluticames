using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Requests.Queries;
using Api.Models.Responses;
using Application.Content.Services;
using Contracts.DTO.Catalogue;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SummitsController(ISummitService _summitService) : ControllerBase
    {
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateSummitsAsync(IEnumerable<CreateSummitRequest> createSummitRequests, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var summitDtos = createSummitRequests
                .ToList()
                .ConvertAll(summit =>
                    new AddNewSummitDto(
                        Name: summit.Name,
                        Altitude: summit.Altitude,
                        Latitude: float.Parse(summit.Location.Split(',').First()),
                        Longitude: float.Parse(summit.Location.Split(',').Last()),
                        IsEssential: summit.IsEssential,
                        RegionName: summit.RegionName));

            // Cridar servei d'aplicació
            var addNewSummitsResult = await _summitService.AddNewSummitsAsync(summitDtos, cancellationToken);

            // Retornar Model/Resposta o error
            return addNewSummitsResult.Match(
                result => Created(string.Empty, new { result }),
                error => error.ToProblemDetails());
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RetrieveSummitsAsync([FromQuery] RetrieveSummitsQuery retrieveSummitsQuery, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var filterDto = new ListSummitsFilterDto(
                Id: retrieveSummitsQuery?.Id,
                Name: retrieveSummitsQuery?.Name,
                Altitude: (retrieveSummitsQuery?.MinAltitude, retrieveSummitsQuery?.MaxAltitude),
                IsEssential: retrieveSummitsQuery?.IsEssential,
                RegionName: retrieveSummitsQuery?.RegionName,
                DifficultyLevel: retrieveSummitsQuery?.DifficultyLevel);

            // Cridar servei d'aplicació
            var listSummitsResult = await _summitService.ListSummitsAsync(filterDto, cancellationToken);

            // Retornar Model/Resposta o error
            return listSummitsResult.Match(
                result =>
                {
                    var retrieveSummitsResponse = result!.ToDictionary(kv => kv.Key, kv =>
                        new RetrieveSummitResponse(
                            Name: kv.Value.Name,
                            Altitude: kv.Value.Altitude,
                            Location: $"{kv.Value.Latitude}, {kv.Value.Longitude}",
                            IsEssential: kv.Value.IsEssential,
                            RegionName: kv.Value.RegionName,
                            DifficultyLevel: kv.Value.DifficultyLevel));

                    return retrieveSummitsResponse.Any() ? Ok(retrieveSummitsResponse) : NoContent();
                },
                error => error.ToProblemDetails());
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateSummitsAsync(IDictionary<Guid, UpdateSummitRequest> udpateSummitRequests, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var summitDtos = udpateSummitRequests.ToDictionary(summit => summit.Key, summit =>
                new ReplaceSummitDetailDto(
                    Name: summit.Value.Name,
                    Altitude: summit.Value.Altitude,
                    Latitude: float.TryParse(summit.Value.Location?.Split(',').First(), out var latitude) ? latitude : null,
                    Longitude: float.TryParse(summit.Value.Location?.Split(',').First(), out var longitude) ? longitude : null,
                    IsEssential: summit.Value.IsEssential,
                    RegionName: summit.Value.RegionName));

            // Cridar servei d'aplicació
            var replaceSummitsResult = await _summitService.ReplaceSummitsAsync(summitDtos, cancellationToken);

            // Retornar Model/Resposta o error
            return replaceSummitsResult.Match(
                result => Accepted(result),
                error => error.ToProblemDetails());
        }

        [HttpDelete]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSummitsAsync(IEnumerable<Guid> summitIds, CancellationToken cancellationToken = default)
        {
            // Cridar servei d'aplicació
            var removeSummitsResult = await _summitService.RemoveSummitsAsync(summitIds, cancellationToken);

            // Retornar Model/Resposta o error
            return removeSummitsResult.Match(
                result => Accepted(result),
                error => error.ToProblemDetails());
        }
    }
}
