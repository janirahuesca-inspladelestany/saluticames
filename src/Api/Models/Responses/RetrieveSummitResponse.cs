namespace Api.Models.Responses;

// Defineix un record per encapsular la resposta dels cims
public record RetrieveSummitResponse(string Name, int Altitude, string Location, bool IsEssential, string RegionName, string DifficultyLevel);
