namespace Api.Models.Requests;

public record UpdateSummitsRequest(IDictionary<Guid, UpdateSummitDetail> Summits);

public record UpdateSummitDetail(int? Altitude, string? Name, string? Location, string? RegionName);

