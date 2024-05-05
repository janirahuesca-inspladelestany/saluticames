namespace Api.Models.Requests;

public record UpdateSummitRequest(string? Name, int? Altitude, string? Location, bool? IsEssential, string? RegionName);

