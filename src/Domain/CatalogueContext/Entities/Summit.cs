using Domain.CatalogueContext.DTO;
using Domain.CatalogueContext.ValueObjects;

namespace Domain.CatalogueContext.Entities;

public sealed class Summit
{
    private int _altitude;

    private Summit() { }

    public static Summit Create(SummitDto summitDto)
    {
        Summit summit = new()
        {
            Altitude = summitDto.Altitude,
            Location = summitDto.Location,
            Name = summitDto.Name,
            Region = summitDto.Region
        };

        return summit;
    }

    public Guid Id { get; private init; } = Guid.NewGuid();

    public string Name { get; internal set; } = null!;

    public int Altitude
    {
        get
        {
            return _altitude;
        }
        internal set
        {
            if (value <= 0 || value > 3150) throw new ArgumentOutOfRangeException(nameof(Altitude));
            _altitude = value;
            CalculateDifficulty();
        }
    }

    public string Location { get; internal set; } = null!;

    public string Region { get; internal set; } = null!;

    public DifficultyLevel Difficulty { get; private set; }

    private void CalculateDifficulty()
    {
        Difficulty = Altitude switch
        {
            < 1500 => DifficultyLevel.EASY,
            >= 1500 and < 2500 => DifficultyLevel.MODERATE,
            >= 2500 => DifficultyLevel.DIFFICULT
        };
    }
}