using Domain.Catalogues.Enums;
using SharedKernel.Abstractions;

namespace Domain.Catalogues.Entities;

public sealed class Summit : Entity<Guid>
{
    private int _altitude;
    private DifficultyLevel _difficultyLevel;

    private Summit(Guid id)
        : base(id)
    {
        _difficultyLevel = GetDifficultyLevel();
    }

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
        }
    }
    public string Location { get; internal set; } = null!;
    public string Name { get; internal set; } = null!;
    public Region Region { get; internal set; } = Region.NONE;
    public DifficultyLevel DifficultyLevel
    {
        get
        {
            return _difficultyLevel;
        }
        internal set
        {
            var expectedDifficultyLevel = GetDifficultyLevel();
            if (value != expectedDifficultyLevel) throw new ArgumentOutOfRangeException(nameof(value));
            _difficultyLevel = value;
        }
    }

    public static Summit Create(int altitude, string location, string name, Region region, Guid? id = null)
    {
        Summit summit = new(id ?? Guid.NewGuid())
        {
            Altitude = altitude,
            Location = location,
            Name = name,
            Region = region,
        };

        return summit;
    }

    private DifficultyLevel GetDifficultyLevel()
    {
        return _altitude switch
        {
            < 1500 => DifficultyLevel.EASY,
            >= 1500 and < 2500 => DifficultyLevel.MODERATE,
            >= 2500 => DifficultyLevel.DIFFICULT
        };
    }
}