namespace Application.CatalogueContext.Contracts;

public record SummitQueryResult(Guid Id, int Altitude, string Name, string Location, string Region, DifficultyLevel Difficulty);
