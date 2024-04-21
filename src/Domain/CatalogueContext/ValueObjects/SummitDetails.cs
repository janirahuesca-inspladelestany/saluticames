namespace Domain.CatalogueContext.ValueObjects;

public record SummitDetails
{
    private int _altitude;

    public int Altitude
    {
        get
        {
            return _altitude;
        }
        init
        {
            if (value <= 0 || value > 3150) throw new ArgumentOutOfRangeException(nameof(Altitude));
            _altitude = value;
        }
    }

    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public string Region { get; init; } = null!;
    public DifficultyLevel Difficulty => CalculateDifficulty();

    private DifficultyLevel CalculateDifficulty() => _altitude switch
    {
        < 1500 => DifficultyLevel.EASY,
        >= 1500 and < 2500 => DifficultyLevel.MODERATE,
        >= 2500 => DifficultyLevel.DIFFICULT
    };
}