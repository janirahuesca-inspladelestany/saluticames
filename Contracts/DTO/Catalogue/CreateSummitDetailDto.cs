namespace Contracts.DTO.Catalogue;

public record CreateSummitDetailDto(string Name, int Altitude, string Latitude, string Longitude, bool IsEssential, string RegionName);

