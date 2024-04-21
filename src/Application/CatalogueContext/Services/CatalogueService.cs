using Application.CatalogueContext.Contracts;
using Application.CatalogueContext.Mappers;
using Domain.CatalogueContext.Entities;
using Domain.CatalogueContext.Repositories;
using Domain.CatalogueContext.Services;

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

    public bool CreateSummits(Guid catalogueId, IEnumerable<SummitCommand> summits)
    {
        if (summits.Any(summit => !_regionService.IsAvailableRegion(summit.Region)))
            throw new ArgumentOutOfRangeException();

        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetById(catalogueId);
        if (catalogue is null) return false;

        // Mapejar DTO a BO
        var summitsDetails = SummitMapper.FromDtoToBo(summits);

        // Afegir cims al catàleg
        var newSummits = catalogue.AddSummits(summitsDetails);

        // Persistir el catàleg
        _catalogueRepository.AddSummits(newSummits);

        return true;
    }

    public bool DeleteSummits(Guid catalogueId, IEnumerable<Guid> summitIds)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetById(catalogueId);
        if (catalogue is null) return false;

        // Eliminar cims del catàleg
        var oldSummits = catalogue.RemoveSummits(summitIds);

        // Persistir el catàleg
        _catalogueRepository.RemoveSummits(oldSummits);

        return true;
    }

    public IEnumerable<SummitQueryResult> ReadSummits(Guid catalogueId, bool ascOrder = true)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetById(catalogueId);
        if (catalogue is null) return Enumerable.Empty<SummitQueryResult>();

        // Recuperar cims del catàleg
        var summitsByAltitude = catalogue.List(ascOrder ? Catalogue.OrderType.ASC : Catalogue.OrderType.DESC);

        // Mapejar de BO a DTO
        return SummitMapper.FromBoToDto(summitsByAltitude);
    }

    public IEnumerable<SummitQueryResult> ReadSummitsByAltitude(Guid catalogueId, int altitude, bool ascOrder = true)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetById(catalogueId);
        if (catalogue is null) return Enumerable.Empty<SummitQueryResult>();

        // Recuperar cims del catàleg
        var summitsByAltitude = catalogue.Filter(altitude, Catalogue.FilterType.ALTITUDE, ascOrder ? Catalogue.OrderType.ASC : Catalogue.OrderType.DESC);

        // Mapejar de BO a DTO
        return SummitMapper.FromBoToDto(summitsByAltitude);
    }

    public IEnumerable<SummitQueryResult> ReadSummitsByDifficulty(Guid catalogueId, DifficultyLevel difficultyLevel, bool ascOrder = true)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetById(catalogueId);
        if (catalogue is null) return Enumerable.Empty<SummitQueryResult>();

        // Mapejar de DTO a BO
        var difficulty = SummitMapper.FromDtoToBo(difficultyLevel);

        // Recuperar cims del catàleg
        var summitsByDifficulty = catalogue.Filter(difficulty, Catalogue.FilterType.DIFICULTY, ascOrder ? Catalogue.OrderType.ASC : Catalogue.OrderType.DESC);

        // Mapejar de BO a DTO
        return SummitMapper.FromBoToDto(summitsByDifficulty);
    }

    public IEnumerable<SummitQueryResult> ReadSummitsByLocation(Guid catalogueId, string location, bool ascOrder = true)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetById(catalogueId);
        if (catalogue is null) return Enumerable.Empty<SummitQueryResult>(); ;

        // Recuperar cims del catàleg
        var summitsByLocation = catalogue.Filter(location, Catalogue.FilterType.LOCATION, ascOrder ? Catalogue.OrderType.ASC : Catalogue.OrderType.DESC);

        // Mapejar de BO a DTO
        return SummitMapper.FromBoToDto(summitsByLocation);
    }

    public IEnumerable<SummitQueryResult> ReadSummitsByName(Guid catalogueId, string name, bool ascOrder = true)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetById(catalogueId);
        if (catalogue is null) return Enumerable.Empty<SummitQueryResult>(); ;

        // Recuperar cims del catàleg
        var summitsByName = catalogue.Filter(name, Catalogue.FilterType.NAME, ascOrder ? Catalogue.OrderType.ASC : Catalogue.OrderType.DESC);

        // Mapejar de BO a DTO
        return SummitMapper.FromBoToDto(summitsByName);
    }

    public IEnumerable<SummitQueryResult> ReadSummitsByRegion(Guid catalogueId, string region, bool ascOrder = true)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetById(catalogueId);
        if (catalogue is null) return Enumerable.Empty<SummitQueryResult>(); ;

        // Recuperar cims del catàleg
        var summitsByRegion = catalogue.Filter(region, Catalogue.FilterType.REGION, ascOrder ? Catalogue.OrderType.ASC : Catalogue.OrderType.DESC);

        // Mapejar de BO a DTO
        return SummitMapper.FromBoToDto(summitsByRegion);
    }

    public bool UpdateSummits(Guid catalogueId, IDictionary<Guid, SummitCommand> summits)
    {
        if (summits.Any(summit => !_regionService.IsAvailableRegion(summit.Value.Region)))
            throw new ArgumentOutOfRangeException();

        // Recuperar el catàleg
        var catalogue = _catalogueRepository.GetById(catalogueId);
        if (catalogue is null) return false;

        // Mapejar de DTO a BO
        var summitsDetails = summits.ToDictionary(summit => summit.Key, summit => SummitMapper.FromDtoToBo(summit.Value));

        // Afegir cims al catàleg
        var newSummits = catalogue.EditSummits(summitsDetails);

        // Persistir el catàleg
        _catalogueRepository.EditSummits(newSummits);

        return true;
    }

    public IEnumerable<CatalogueQueryResult> GetCatalogues()
    {
        // Recuperar catàlegs
        var catalogues = _catalogueRepository.GetAll();

        // Mapejar de BO a DTO
        return CatalogueMapper.FromBoToDto(catalogues);
    }
}
