namespace Contracts.DTO.Challenge;

// Data Transfer Object (DTO) per afegir detalls d'una nova ascensió
public record AddNewClimbDetailDto(Guid SummitId, DateTime? AscensionDateTime);

