using Domain.CatalogueContext.DTO;
using Domain.CatalogueContext.Entities;
using Domain.CatalogueContext.ValueObjects;

namespace Application.CatalogueContext.Services;

public class CatalogueService : ICatalogueService
{
    private readonly Catalogue _catalogueFake;

    public CatalogueService()
    {
        _catalogueFake = new Catalogue();
        _catalogueFake.AddSummit(new SummitDto("Name1", 1000, "Location1", "Region1"));
        _catalogueFake.AddSummit(new SummitDto("Name2", 2000, "Location2", "Region2"));
        _catalogueFake.AddSummit(new SummitDto("Name3", 3000, "Location3", "Region3"));
    }

    public void CreateSummits(IEnumerable<SummitDto> summits)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueFake;

        // Afegir cims al catàleg
        catalogue.AddSummits(summits);

        // Persistir el catàleg
        // TODO: Pending to create catolgue repository
    }

    public void DeleteSummits(IEnumerable<Guid> ids)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueFake;

        // Eliminar cims del catàleg
        catalogue.RemoveSummits(ids);

        // Persistir el catàleg
        // TODO: Pending to create catolgue repository
    }

    public void ReadSummitsByAltitude(int altitude, Catalogue.OrderType order = Catalogue.OrderType.ASC)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueFake;

        // Recuperar cims del catàleg
        catalogue.List(altitude, Catalogue.FilterType.ALTITUDE, order);
    }

    public void ReadSummitsByDifficulty(DifficultyLevel difficulty, Catalogue.OrderType order = Catalogue.OrderType.ASC)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueFake;

        // Recuperar cims del catàleg
        catalogue.List(difficulty, Catalogue.FilterType.DIFICULTY, order);
    }

    public void ReadSummitsByLocation(string location, Catalogue.OrderType order = Catalogue.OrderType.ASC)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueFake;

        // Recuperar cims del catàleg
        catalogue.List(location, Catalogue.FilterType.LOCATION, order);
    }

    public void ReadSummitsByName(string name, Catalogue.OrderType order = Catalogue.OrderType.ASC)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueFake;

        // Recuperar cims del catàleg
        catalogue.List(name, Catalogue.FilterType.NAME, order);
    }

    public void ReadSummitsByRegion(string region, Catalogue.OrderType order = Catalogue.OrderType.ASC)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueFake;

        // Recuperar cims del catàleg
        catalogue.List(region, Catalogue.FilterType.REGION, order);
    }

    public void UpdateSummits(IDictionary<Guid, SummitDto> summits)
    {
        // Recuperar el catàleg
        var catalogue = _catalogueFake;

        // Afegir cims al catàleg
        catalogue.EditSummits(summits);

        // Persistir el catàleg
        // TODO: Pending to create catolgue repository
    }
}
