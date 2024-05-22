namespace Contracts.DTO.Content;

// Data Transfer Object (DTO) per a afegir un nou cim
public record AddNewSummitDto(string Name, int Altitude, float Latitude, float Longitude, bool IsEssential, string RegionName);

