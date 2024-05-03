using Api.Extensions;
using Api.Models.Requests;
using Api.Models.Requests.Queries;
using Application.CatalogueContext.Services;
using Contracts.DTO.Catalogue;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/{id}/summits")]
    [ApiController]
    public class CatalogueController(ICatalogueService _catalogueService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateSummitsAsync(Guid id, CreateSummitsRequest request, CancellationToken cancellationToken = default)
        {
            var summitsToCreate = request.Summits
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
        public async Task<IActionResult> ReadSummitsAsync(Guid id, [FromQuery] ReadSummitsQuery summitsQuery, CancellationToken cancellationToken = default)
        {
            var filter = new GetSummitsFilterDto(
                Id: summitsQuery.Id,
                Altitude: (summitsQuery.MinAltitude, summitsQuery.MaxAltitude),
                Name: summitsQuery.Name,
                Location: summitsQuery.Location,
                RegionName: summitsQuery.RegionName);

            var result = await _catalogueService.GetSummitsAsync(catalogueId: id, filter, cancellationToken);

            return result.Match(
                result => Ok(result),
                error => result.ToProblemDetails());
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSummitsAsync(Guid id, UpdateSummitsRequest request, CancellationToken cancellationToken = default)
        {
            var summitsToUpdate = request.Summits.ToDictionary(summit => summit.Key, summit =>
                new ReplaceSummitDetailDto(
                    Altitude: summit.Value.Altitude,
                    Name: summit.Value.Name,
                    Location: summit.Value.Location,
                    RegionName: summit.Value.RegionName));

            var result = await _catalogueService.ReplaceSummitsAsync(catalogueId: id, summitsToUpdate, cancellationToken);

            return result.Match(
                result => Ok(result),
                error => result.ToProblemDetails());
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSummitsAsync(Guid id, IEnumerable<Guid> summitIdsToDelete, CancellationToken cancellationToken = default)
        {
            var result = await _catalogueService.RemoveSummitsAsync(catalogueId: id, summitIdsToDelete, cancellationToken);

            return result.Match(
                result => Ok(result),
                error => result.ToProblemDetails());
        }
    }
}
