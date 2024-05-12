using Api.Extensions;
using Application.Content.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/{id:guid}")]
    [ApiController]
    public class CatalogueController(ICatalogueService _catalogueService) : ControllerBase
    {
        [HttpPost("summits")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateCatalogueSummitsAsync(Guid id, IEnumerable<Guid> summitIds, CancellationToken cancellationToken = default)
        {
            // Cridar servei d'aplicació
            var addNewSummitIdsInCatalogueResult = await _catalogueService.AddNewSummitIdsInCatalogueAsync(id, summitIds, cancellationToken);

            // Retornar Model/Resposta o error
            return addNewSummitIdsInCatalogueResult.Match(
                () => Created(string.Empty, default),
                error => error.ToProblemDetails());
        }

        [HttpGet("summits")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RetrieveCatalogueSummitsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Cridar servei d'aplicació
            var listSummitIdsFromCatalogueResult = await _catalogueService.ListSummitIdsFromCatalogueAsync(id, cancellationToken);

            // Retornar Model/Resposta o error
            return listSummitIdsFromCatalogueResult.Match(
                result => result!.Any() ? Ok(result) : NoContent(),
                error => error.ToProblemDetails());
        }

        [HttpDelete("summits")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCatalogueSummitsAsync(Guid id, IEnumerable<Guid> summitIds, CancellationToken cancellationToken = default)
        {
            // Cridar servei d'aplicació
            var removeSummitIdsFromCatalogueResult = await _catalogueService.RemoveSummitIdsFromCatalogueAsync(id, summitIds, cancellationToken);

            // Retornar Model/Resposta o error
            return removeSummitIdsFromCatalogueResult.Match(
                () => Accepted(),
                error => error.ToProblemDetails());
        }
    }
}
