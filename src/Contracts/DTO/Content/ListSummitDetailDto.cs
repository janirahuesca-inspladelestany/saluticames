namespace Contracts.DTO.Content;

public record ListSummitDetailDto(string Name, int Altitude, float Latitude, float Longitude, bool IsEssential, string RegionName, string DifficultyLevel);

