using Api.Extensions;
using Api.Models;
using Application.Catalogues.Services;
using Contracts.Catalogues.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/{id}/summit")]
    [ApiController]
    public class CatalogueController(ICatalogueService _catalogueService) : ControllerBase
    {
        [HttpPost]
        public async Task<IResult> CreateSummitsAsync(Guid id, CreateSummitsRequest request, CancellationToken cancellationToken = default)
        {
            var summitsToCreate = request.Summits
                .ToList()
                .ConvertAll(summit =>
                    new SummitDto(
                        Altitude: summit.Altitude,
                        Name: summit.Name,
                        Location: summit.Location,
                        RegionName: summit.RegionName));

            var result = await _catalogueService.CreateSummitsAsync(catalogueId: id, summitsToCreate, cancellationToken);

            return result.Match(
                result => Results.Ok(result),
                error => result.ToProblemDetails());
        }

        [HttpGet("{summitId}")]
        public async Task<IResult> ReadSummitsAsync(Guid id, ReadSummitsRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _catalogueService.GetSummitsAsync(catalogueId: id, cancellationToken);

            return result.Match(
                result => Results.Ok(result),
                error => result.ToProblemDetails());
        }

        [HttpPut]
        public async Task<IResult> UpdateSummitsAsync(Guid id, UpdateSummitsRequest request, CancellationToken cancellationToken = default)
        {
            var summitsToUpdate = request.Summits.ToDictionary(summit => summit.Key, summit =>
                new SummitDto(
                    Altitude: summit.Value.Altitude,
                    Name: summit.Value.Name,
                    Location: summit.Value.Location,
                    RegionName: summit.Value.RegionName));

            var result = await _catalogueService.ReplaceSummitsAsync(catalogueId: id, summitsToUpdate, cancellationToken);

            return result.Match(
                result => Results.Ok(result),
                error => result.ToProblemDetails());
        }

        [HttpDelete]
        public async Task<IResult> DeleteSummitsAsync(Guid id, IEnumerable<Guid> summitIdsToDelete, CancellationToken cancellationToken = default)
        {
            var result = await _catalogueService.RemoveSummitsAsync(catalogueId: id, summitIdsToDelete, cancellationToken);

            return result.Match(
                result => Results.Ok(result),
                error => result.ToProblemDetails());
        }
    }
}
