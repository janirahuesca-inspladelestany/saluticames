using Domain.CatalogueContext.DTO;
using Domain.CatalogueContext.Entities;
using Domain.CatalogueContext.ValueObjects;


namespace Application.CatalogueContext.Services;

public interface ICatalogueService
{
    void CreateSummits(Guid catalogueId, IEnumerable<SummitDto> summits);
    IEnumerable<Summit> ReadSummitsByName(Guid catalogueId, string name, Catalogue.OrderType order = Catalogue.OrderType.ASC);
    IEnumerable<Summit> ReadSummitsByAltitude(Guid catalogueId, int altitude, Catalogue.OrderType order = Catalogue.OrderType.ASC);
    IEnumerable<Summit> ReadSummitsByLocation(Guid catalogueId, string location, Catalogue.OrderType order = Catalogue.OrderType.ASC);
    IEnumerable<Summit> ReadSummitsByRegion(Guid catalogueId, string region, Catalogue.OrderType order = Catalogue.OrderType.ASC);
    IEnumerable<Summit> ReadSummitsByDifficulty(Guid catalogueId, DifficultyLevel difficulty, Catalogue.OrderType order = Catalogue.OrderType.ASC);
    void UpdateSummits(Guid catalogueId, IDictionary<Guid, SummitDto> summitsToUpdate);
    void DeleteSummits(Guid catalogueId, IEnumerable<Guid> ids);
}