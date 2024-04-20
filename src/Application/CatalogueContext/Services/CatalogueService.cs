using Domain.CatalogueContext.DTO;
using Domain.CatalogueContext.Entities;
using Domain.CatalogueContext.Repositories;
using Domain.CatalogueContext.Services;
using Domain.CatalogueContext.ValueObjects;

namespace Application.CatalogueContext.Services;

public class CatalogueService : ICatalogueService
{
    private readonly IRegionService _regionService;
    private readonly ICatalogueRepository _catalogueRepository;

    public CatalogueService(
        IRegionService regionService,
        ICatalogueRepository catalogueRepository)
    {
        _regionService = regionService;
        _catalogueRepository = catalogueRepository;
    }

    public void CreateSummits(Guid catalogueId, IEnumerable<SummitDto> summits)
    {
        if (summits.Any(summit => !_regionService.IsAvailableRegion(summit.Region)))
            throw new ArgumentOutOfRangeException(nameof(SummitDto.Region));

        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetCatalogue(catalogueId);
        if (catalogue is null) return;

        // Afegir cims al catàleg
        catalogue.AddSummits(summits);

        // Persistir el catàleg
        // TODO: Pending to create catolgue repository
    }

    public void DeleteSummits(Guid catalogueId, IEnumerable<Guid> summitIds)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetCatalogue(catalogueId);
        if (catalogue is null) return;

        // Eliminar cims del catàleg
        catalogue.RemoveSummits(summitIds);

        // Persistir el catàleg
        // TODO: Pending to create catolgue repository
    }

    public IEnumerable<Summit> ReadSummitsByAltitude(Guid catalogueId, int altitude, Catalogue.OrderType order = Catalogue.OrderType.ASC)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetCatalogue(catalogueId);
        if (catalogue is null) return Enumerable.Empty<Summit>(); ;

        // Recuperar cims del catàleg
        return catalogue.List(altitude, Catalogue.FilterType.ALTITUDE, order);
    }

    public IEnumerable<Summit> ReadSummitsByDifficulty(Guid catalogueId, DifficultyLevel difficulty, Catalogue.OrderType order = Catalogue.OrderType.ASC)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetCatalogue(catalogueId);
        if (catalogue is null) return Enumerable.Empty<Summit>();

        // Recuperar cims del catàleg
        return catalogue.List(difficulty, Catalogue.FilterType.DIFICULTY, order);
    }

    public IEnumerable<Summit> ReadSummitsByLocation(Guid catalogueId, string location, Catalogue.OrderType order = Catalogue.OrderType.ASC)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetCatalogue(catalogueId);
        if (catalogue is null) return Enumerable.Empty<Summit>(); ;

        // Recuperar cims del catàleg
        return catalogue.List(location, Catalogue.FilterType.LOCATION, order);
    }

    public IEnumerable<Summit> ReadSummitsByName(Guid catalogueId, string name, Catalogue.OrderType order = Catalogue.OrderType.ASC)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetCatalogue(catalogueId);
        if (catalogue is null) return Enumerable.Empty<Summit>(); ;

        // Recuperar cims del catàleg
        return catalogue.List(name, Catalogue.FilterType.NAME, order);
    }

    public IEnumerable<Summit> ReadSummitsByRegion(Guid catalogueId, string region, Catalogue.OrderType order = Catalogue.OrderType.ASC)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetCatalogue(catalogueId);
        if (catalogue is null) return Enumerable.Empty<Summit>(); ;

        // Recuperar cims del catàleg
        return catalogue.List(region, Catalogue.FilterType.REGION, order);
    }

    public void UpdateSummits(Guid catalogueId, IDictionary<Guid, SummitDto> summits)
    {
        if (summits.Any(summit => !_regionService.IsAvailableRegion(summit.Value.Region)))
            throw new ArgumentOutOfRangeException(nameof(SummitDto.Region));

        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetCatalogue(catalogueId);
        if (catalogue is null) return;

        // Afegir cims al catàleg
        catalogue.EditSummits(summits);

        // Persistir el catàleg
        // TODO: Pending to create catolgue repository
    }
}
