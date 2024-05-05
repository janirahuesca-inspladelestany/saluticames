namespace Api.Models.Requests;

public record CreateSummitRequest(string Name, int Altitude, string Location, bool IsEssential, string RegionName);
