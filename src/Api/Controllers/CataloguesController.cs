using Api.Extensions;
using Api.Models.Requests.Queries;
using Application.Content.Services;
using Contracts.DTO.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]"), Authorize]
    [ApiController]
    public class CataloguesController(ICatalogueService _catalogueService) : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrieveCataloguesAsync([FromQuery] RetrieveCataloguesQuery retrieveCatalogueQuery, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var filter = new ListCataloguesFilterDto(retrieveCatalogueQuery.Id, retrieveCatalogueQuery.Name);

            // Cridar servei d'aplicació
            var getCataloguesResult = await _catalogueService.ListCatalogues(filter, cancellationToken);
            
            // Retornar Model/Resposta o error
            return getCataloguesResult.Match(
                result => result!.Any() ? Ok(result) : NoContent(),
                error => error.ToProblemDetails());
        }
    }
}
