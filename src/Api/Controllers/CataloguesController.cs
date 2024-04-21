using Application.CatalogueContext.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CataloguesController : ControllerBase
    {
        private readonly ICatalogueService _catalogueService;

        public CataloguesController(ICatalogueService catalogueService)
        {
            _catalogueService = catalogueService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var catalogues = _catalogueService.GetCatalogues();
            if(catalogues.IsNullOrEmpty()) return NoContent();
            return Ok(catalogues);
        }
    }
}
