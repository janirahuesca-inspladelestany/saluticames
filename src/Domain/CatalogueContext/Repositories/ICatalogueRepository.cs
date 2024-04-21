using Domain.CatalogueContext.Entities;

namespace Domain.CatalogueContext.Repositories
{
    public interface ICatalogueRepository
    {
        Catalogue? GetById(Guid id);
        IEnumerable<Catalogue> GetAll();
        IEnumerable<string> GetAvailableRegions();
        void AddSummits(IEnumerable<Summit> summits);
        void RemoveSummits(IEnumerable<Guid> summitIds);
        void EditSummits(IEnumerable<Summit> summits);
    }
}
