namespace Contracts.DTO.Catalogue;

public record CreateSummitDetailDto(string Name, int Altitude, float Latitude, float Longitude, bool IsEssential, string RegionName);

