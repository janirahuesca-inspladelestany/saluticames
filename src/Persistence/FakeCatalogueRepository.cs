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

    public Catalogue? GetById(Guid id)
    {
        return _catalogues.GetValueOrDefault(id);
    }

    public IEnumerable<Catalogue> GetAll()
    {
        return _catalogues.Any() ? _catalogues.Select(kv => kv.Value) : Enumerable.Empty<Catalogue>();
    }

    public IEnumerable<string> GetAvailableRegions()
    {
        return ["Pla de l'Estany", "Garrotxa", "Gironès"];
    }
}
