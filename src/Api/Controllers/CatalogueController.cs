﻿using Application.CatalogueContext.Contracts;
using Application.CatalogueContext.Services;
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
        public IActionResult Post(Guid id, IEnumerable<SummitCommand> summits)
        {
            var isSuccessful = _catalogueService.CreateSummits(id, summits);
            
            if (!isSuccessful) 
            {
                return new EmptyResult(); 
            }
            else
            {
                return Ok();
            }
        }

        [HttpDelete]
        public IActionResult Delete(Guid id, IEnumerable<Guid> summitIds)
        {
            var isSuccessful = _catalogueService.DeleteSummits(id, summitIds);

            if (!isSuccessful)
            {
                return new EmptyResult();
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        public IActionResult Get(Guid id, int? altitude, string? name, string? location, string? region, bool asc = true)
        {
            var summits = _catalogueService.ReadSummits(id, asc);

            if (altitude.HasValue)
            {
                var summitsByAltitude = _catalogueService.ReadSummitsByAltitude(id, altitude.Value, asc);
                summits = summits.Intersect(summitsByAltitude);
            }

            if (!string.IsNullOrEmpty(name))
            {
                var summitsByName = _catalogueService.ReadSummitsByName(id, name, asc);
                summits = summits.Intersect(summitsByName);
            }

            if (!string.IsNullOrEmpty(location))
            {
                var summitsByLocation = _catalogueService.ReadSummitsByLocation(id, location, asc);
                summits = summits.Intersect(summitsByLocation);
            }

            if (!string.IsNullOrEmpty(region))
            {
                var summitsByRegion = _catalogueService.ReadSummitsByRegion(id, region, asc);
                summits = summits.Intersect(summitsByRegion);
            }

            if (!summits.Any())
            {
                return NoContent();
            }

            return Ok(summits);
        }

        [HttpPut]
        public IActionResult Put(Guid id, IDictionary<Guid, SummitCommand> summits)
        {
            var isSuccessful = _catalogueService.UpdateSummits(id, summits);

            if (!isSuccessful)
            {
                return new EmptyResult();
            }
            else
            {
                return Ok();
            }
        }
    }
}
