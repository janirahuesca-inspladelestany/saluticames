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
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ReadCataloguesAsync([FromQuery] ReadCataloguesQuery cataloguesQuery, CancellationToken cancellationToken = default)
        {
            // Mapejar Model/Request a Contract/DTO
            var filter = new GetCataloguesFilterDto(cataloguesQuery.Id, cataloguesQuery.Name);

            // Cridar servei d'aplicació
            var getCataloguesResult = await _catalogueService.GetCatalogues(filter, cancellationToken);
            
            // Retornar Model/Resposta o error
            return getCataloguesResult.Match(
                result => result!.Any() ? Ok(result) : NoContent(),
                error => error.ToProblemDetails());
        }
    }
}
