using Application.CatalogueContext.Contracts;

namespace Application.CatalogueContext.Services;

public interface ICatalogueService
{
    void CreateSummits(Guid catalogueId, IEnumerable<SummitCommand> summits);
    IEnumerable<SummitQueryResult> ReadSummits(Guid catalogueId, bool ascOrder = true);
    IEnumerable<SummitQueryResult> ReadSummitsByName(Guid catalogueId, string name, bool ascOrder = true);
    IEnumerable<SummitQueryResult> ReadSummitsByAltitude(Guid catalogueId, int altitude, bool ascOrder = true);
    IEnumerable<SummitQueryResult> ReadSummitsByLocation(Guid catalogueId, string location, bool ascOrder = true);
    IEnumerable<SummitQueryResult> ReadSummitsByRegion(Guid catalogueId, string region, bool ascOrder = true);
    IEnumerable<SummitQueryResult> ReadSummitsByDifficulty(Guid catalogueId, DifficultyLevel difficulty, bool ascOrder = true);
    void UpdateSummits(Guid catalogueId, IDictionary<Guid, SummitCommand> summitsToUpdate);
    void DeleteSummits(Guid catalogueId, IEnumerable<Guid> ids);
    IEnumerable<CatalogueQueryResult> GetCatalogues();
}