namespace Contracts.DTO.Challenge;

// Data Transfer Object (DTO) per afegir detalls d'un nou diari
public record AddNewDiaryDto(string Name, string HikerId, Guid CatalogueId);

