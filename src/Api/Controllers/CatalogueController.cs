using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Requests.Queries;
using Application.CatalogueContext.Services;
using Contracts.DTO.Catalogue;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/{id}")]
    [ApiController]
    public class CatalogueController(ICatalogueService _catalogueService) : ControllerBase
    {
        [HttpPost("summits")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateSummitsAsync(Guid id, IEnumerable<CreateSummitRequest> createSummitRequests, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var summitsToCreate = createSummitRequests
                .ToList()
                .ConvertAll(summit =>
                    new CreateSummitDetailDto(
                        Name: summit.Name,
                        Altitude: summit.Altitude,
                        Latitude: summit.Location.Split(',').First(),
                        Longitude: summit.Location.Split(',').Last(),
                        IsEssential: summit.IsEssential,
                        RegionName: summit.RegionName));

            // Cridar servei d'aplicació
            var createSummitsResult = await _catalogueService.CreateSummitsAsync(catalogueId: id, summitsToCreate, cancellationToken);

            // Retornar Model/Resposta o error
            return createSummitsResult.Match(
                result => Created(string.Empty, new { createdSummitIds = result }),
                error => error.ToProblemDetails());
        }

        [HttpGet("summits")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ReadSummitsAsync(Guid id, [FromQuery] ReadSummitsQuery readSummitsQuery, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var filter = new GetSummitsFilterDto(
                Id: readSummitsQuery?.Id,
                Name: readSummitsQuery?.Name,
                Altitude: (readSummitsQuery?.MinAltitude, readSummitsQuery?.MaxAltitude),
                IsEssential: readSummitsQuery?.IsEssential,
                RegionName: readSummitsQuery?.RegionName,
                DifficultyLevel: readSummitsQuery?.DifficultyLevel);

            // Cridar servei d'aplicació
            var getSummitsResult = await _catalogueService.GetSummitsAsync(catalogueId: id, filter, cancellationToken);

            // Retornar Model/Resposta o error
            return getSummitsResult.Match(
                result => 
                {
                    var readSummitsResponse = result!.ToDictionary(kv => kv.Key, kv =>
                        new ReadSummitResponse(
                            Name: kv.Value.Name,
                            Altitude: kv.Value.Altitude,
                            Location: $"{kv.Value.Latitude}, {kv.Value.Longitude}",
                            IsEssential: kv.Value.IsEssential,
                            RegionName: kv.Value.RegionName,
                            DifficultyLevel: kv.Value.DifficultyLevel));

                    return readSummitsResponse.Any() ? Ok(readSummitsResponse) : NoContent();
                },
                error => error.ToProblemDetails());
        }

        [HttpPut("summits")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateSummitsAsync(Guid id, IDictionary<Guid, UpdateSummitRequest> udpateSummitRequests, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var summitsToUpdate = udpateSummitRequests.ToDictionary(summit => summit.Key, summit =>
                new ReplaceSummitDetailDto(
                    Name: summit.Value.Name,
                    Altitude: summit.Value.Altitude,
                    Latitude: summit.Value.Location?.Split(',').First(),
                    Longitude: summit.Value.Location?.Split(',').Last(),
                    IsEssential: summit.Value.IsEssential,
                    RegionName: summit.Value.RegionName));

            // Cridar servei d'aplicació
            var replaceSummitsResult = await _catalogueService.ReplaceSummitsAsync(catalogueId: id, summitsToUpdate, cancellationToken);

            // Retornar Model/Resposta o error
            return replaceSummitsResult.Match(
                result => Accepted(result),
                error => error.ToProblemDetails());
        }

        [HttpDelete("summits")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSummitsAsync(Guid id, IEnumerable<Guid> summitIdsToDelete, CancellationToken cancellationToken = default)
        {
            // Cridar servei d'aplicació
            var removeSummitsResult = await _catalogueService.RemoveSummitsAsync(catalogueId: id, summitIdsToDelete, cancellationToken);

            // Retornar Model/Resposta o error
            return removeSummitsResult.Match(
                result => Accepted(result),
                error => error.ToProblemDetails());
        }
    }
}
