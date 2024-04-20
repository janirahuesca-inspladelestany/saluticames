using Domain.CatalogueContext.Entities;
using Domain.CatalogueContext.Repositories;

namespace Persistence;

public class FakeCatalogueRepository : ICatalogueRepository
{
    private readonly Dictionary<Guid, Catalogue> _catalogues;

    public FakeCatalogueRepository()
    {
        var catalogue = new Catalogue(default);
        _catalogues = new()
        {
            [catalogue.Id] = catalogue
        };
    }

    public Catalogue? GetCatalogue(Guid id)
    {
        return _catalogues.GetValueOrDefault(id);
    }

    public IEnumerable<string> GetAvailableRegions()
    {
        return ["Pla de l'Estany", "Garrotxa", "Gironès"];
    }
}
