namespace Contracts.DTO.Content;

// Data Transfer Object (DTO) per a detallar la substitució de cims
public record ReplaceSummitDetailDto(string? Name, int? Altitude, float? Latitude, float? Longitude, bool? IsEssential, string? RegionName);

