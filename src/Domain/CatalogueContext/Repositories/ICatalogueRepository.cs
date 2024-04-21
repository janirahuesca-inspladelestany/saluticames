using Domain.CatalogueContext.Entities;

namespace Domain.CatalogueContext.Repositories
{
    public interface ICatalogueRepository
    {
        Catalogue? GetById(Guid id);
        IEnumerable<Catalogue> GetAll();
        IEnumerable<string> GetAvailableRegions();
    }
}
