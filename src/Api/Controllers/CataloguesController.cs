using Api.Extensions;
using Api.Models.Requests.Queries;
using Application.CatalogueContext.Services;
using Contracts.DTO.Catalogue;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CataloguesController(ICatalogueService _catalogueService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ReadCataloguesAsync([FromQuery] ReadCataloguesQuery cataloguesQuery, CancellationToken cancellationToken = default)
        {
            var filter = new GetCataloguesFilterDto(cataloguesQuery.Id, cataloguesQuery.Name);
            var result = await _catalogueService.GetCatalogues(filter, cancellationToken);

            return result.Match(
                result => Ok(result),
                error => result.ToProblemDetails());
        }
    }
}
