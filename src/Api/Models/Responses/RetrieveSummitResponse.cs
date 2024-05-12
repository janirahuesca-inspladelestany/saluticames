namespace Api.Models.Responses;

public record RetrieveSummitResponse(string Name, int Altitude, string Location, bool IsEssential, string RegionName, string DifficultyLevel);
