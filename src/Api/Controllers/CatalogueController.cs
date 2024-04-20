using Application.CatalogueContext.Services;
using Domain.CatalogueContext.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/{id}/summit")]
    [ApiController]
    public class CatalogueController : ControllerBase
    {
        private readonly ICatalogueService _catalogueService;

        public CatalogueController(ICatalogueService catalogueService)
        {
            _catalogueService = catalogueService;
        }

        [HttpPost]
        public void Post(Guid id, IEnumerable<SummitDto> summits)
        {
            _catalogueService.CreateSummits(id, summits);
        }

        [HttpDelete]
        public void Delete(Guid id, IEnumerable<Guid> summitIds)
        {
            _catalogueService.DeleteSummits(id, summitIds);
        }

        [HttpGet("altitude")]
        public IActionResult GetByAltitude(Guid id, int value, bool asc = true)
        {
            var summitsByAltitude = _catalogueService.ReadSummitsByAltitude(id,
                value,
                order: asc ? Domain.CatalogueContext.Entities.Catalogue.OrderType.ASC : Domain.CatalogueContext.Entities.Catalogue.OrderType.DESC);

            if(!summitsByAltitude.Any()) 
            {
                return NoContent();
            }

            return Ok(summitsByAltitude);
        }

        [HttpPut]
        public void Put(Guid id, IDictionary<Guid, SummitDto> summits)
        {
            _catalogueService.UpdateSummits(id, summits);
        }
    }
}
