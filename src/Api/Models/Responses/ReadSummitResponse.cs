namespace Api.Models.Requests;

public record ReadSummitResponse(string Name, int Altitude, string Location, bool IsEssential, string RegionName, string DifficultyLevel);
