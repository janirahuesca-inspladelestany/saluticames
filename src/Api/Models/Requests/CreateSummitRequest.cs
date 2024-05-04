namespace Api.Models.Requests;

public record CreateSummitRequest(int Altitude, string Name, string Location, string RegionName);
