using Domain.CatalogueContext.DTO;
using Domain.CatalogueContext.Entities;
using Domain.CatalogueContext.ValueObjects;


namespace Application.CatalogueContext.Services;

public interface ICatalogueService
{
    void CreateSummits(IEnumerable<SummitDto> summits);
    void ReadSummitsByName(string name, Catalogue.OrderType order = Catalogue.OrderType.ASC);
    void ReadSummitsByAltitude(int altitude, Catalogue.OrderType order = Catalogue.OrderType.ASC);
    void ReadSummitsByLocation(string location, Catalogue.OrderType order = Catalogue.OrderType.ASC);
    void ReadSummitsByRegion(string region, Catalogue.OrderType order = Catalogue.OrderType.ASC);
    void ReadSummitsByDifficulty(DifficultyLevel difficulty, Catalogue.OrderType order = Catalogue.OrderType.ASC);
    void UpdateSummits(IDictionary<Guid, SummitDto> summitsToUpdate);
    void DeleteSummits(IEnumerable<Guid> ids);
}