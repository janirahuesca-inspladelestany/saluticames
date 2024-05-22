namespace Contracts.DTO.Content;

// Data Transfer Object (DTO) per a detalls d'un cim
public record ListSummitDetailDto(string Name, int Altitude, float Latitude, float Longitude, bool IsEssential, string RegionName, string DifficultyLevel);

