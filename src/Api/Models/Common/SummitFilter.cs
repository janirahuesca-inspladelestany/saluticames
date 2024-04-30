namespace Api.Models.Common;

public record SummitFilter(
    (int? MinAltitude, int? MaxAltitude)? Altitude,
    string? Name,
    string? Location,
    string? RegionName,
    SummitFilter.DifficultyLevelType? DifficultyLevel)
{
    public enum DifficultyLevelType { LOW, MEDIUM, HIGH }
}
