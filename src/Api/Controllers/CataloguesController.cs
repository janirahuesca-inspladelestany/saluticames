using Api.Extensions;
using Application.Catalogues.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CataloguesController(ICatalogueService _catalogueService) : ControllerBase
    {
        [HttpGet]
        public async Task<IResult> GetCataloguesAsync(CancellationToken cancellationToken = default)
        {
            var result = await _catalogueService.GetCataloguesAsync(cancellationToken);

            return result.Match(
                result => Results.Ok(result),
                error => result.ToProblemDetails());
        }
    }
}
