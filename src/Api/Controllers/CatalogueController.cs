using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Requests.Queries;
using Application.CatalogueContext.Services;
using Contracts.DTO.Catalogue;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/{id}/summits")]
    [ApiController]
    public class CatalogueController(ICatalogueService _catalogueService) : ControllerBase
    {
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateSummitsAsync(Guid id, IEnumerable<CreateSummitRequest> createSummitRequests, CancellationToken cancellationToken = default)
        {
            var summitsToCreate = createSummitRequests
                .ToList()
                .ConvertAll(summit =>
                    new CreateSummitDetailDto(
                        Altitude: summit.Altitude,
                        Name: summit.Name,
                        Location: summit.Location,
                        RegionName: summit.RegionName));

            var result = await _catalogueService.CreateSummitsAsync(catalogueId: id, summitsToCreate, cancellationToken);

            return result.Match(
                result => Ok(result),
                error => result.ToProblemDetails());
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ReadSummitsAsync(Guid id, [FromQuery] ReadSummitsQuery readSummitsQuery, CancellationToken cancellationToken = default)
        {
            var filter = new GetSummitsFilterDto(
                Id: readSummitsQuery.Id,
                Altitude: (readSummitsQuery.MinAltitude, readSummitsQuery.MaxAltitude),
                Name: readSummitsQuery.Name,
                Location: readSummitsQuery.Location,
                RegionName: readSummitsQuery.RegionName);

            var result = await _catalogueService.GetSummitsAsync(catalogueId: id, filter, cancellationToken);

            return result.Match(
                result => Ok(result),
                error => result.ToProblemDetails());
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateSummitsAsync(Guid id, IDictionary<Guid, UpdateSummitRequest> udpateSummitRequests, CancellationToken cancellationToken = default)
        {
            var summitsToUpdate = udpateSummitRequests.ToDictionary(summit => summit.Key, summit =>
                new ReplaceSummitDetailDto(
                    Altitude: summit.Value.Altitude,
                    Name: summit.Value.Name,
                    Location: summit.Value.Location,
                    RegionName: summit.Value.RegionName));

            var result = await _catalogueService.ReplaceSummitsAsync(catalogueId: id, summitsToUpdate, cancellationToken);

            return result.Match(
                result => Accepted(result),
                error => result.ToProblemDetails());
        }

        [HttpDelete]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSummitsAsync(Guid id, IEnumerable<Guid> summitIdsToDelete, CancellationToken cancellationToken = default)
        {
            var result = await _catalogueService.RemoveSummitsAsync(catalogueId: id, summitIdsToDelete, cancellationToken);

            return result.Match(
                result => Accepted(result),
                error => result.ToProblemDetails());
        }
    }
}
