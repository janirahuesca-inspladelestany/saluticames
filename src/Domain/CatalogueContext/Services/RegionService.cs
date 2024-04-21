using Domain.CatalogueContext.Repositories;

namespace Domain.CatalogueContext.Services;

public class RegionService : IRegionService
{
    private readonly ICatalogueRepository _catalogueRepository;

    public RegionService(ICatalogueRepository catalogueRepository)
    {
        _catalogueRepository = catalogueRepository;
    }

    public bool IsAvailableRegion(string regionName)
    {
        var availableRegions = _catalogueRepository.GetAvailableRegions();
        return availableRegions.Contains(regionName, StringComparer.OrdinalIgnoreCase);
    }
}
