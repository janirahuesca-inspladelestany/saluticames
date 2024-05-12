namespace Contracts.DTO.Catalogue;

public record AddNewSummitDto(string Name, int Altitude, float Latitude, float Longitude, bool IsEssential, string RegionName);

