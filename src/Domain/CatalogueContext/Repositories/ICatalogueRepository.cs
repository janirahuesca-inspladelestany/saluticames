using Domain.CatalogueContext.DTO;
using Domain.CatalogueContext.Entities;

namespace Domain.CatalogueContext.Repositories
{
    public interface ICatalogueRepository
    {
        Catalogue? GetCatalogue(Guid id);
        IEnumerable<string> GetAvailableRegions();
    }
}
