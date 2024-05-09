namespace Contracts.DTO.Catalogue;

public record GetSummitDetailDto(string Name, int Altitude, float Latitude, float Longitude, bool IsEssential, string RegionName, string DifficultyLevel);

