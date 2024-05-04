namespace Api.Models.Requests;

public record UpdateSummitRequest(int? Altitude, string? Name, string? Location, string? RegionName);

