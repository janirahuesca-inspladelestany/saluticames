using Domain.CatalogueContext.Enums;
using Domain.CatalogueContext.Errors;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.CatalogueContext.Entities;

public sealed class Summit : Entity<Guid>
{
    private int _altitude;
    private DifficultyLevel _difficultyLevel;
    private Region _region;

    private Summit(Guid id)
        : base(id)
    {
    }

    public string Location { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public DifficultyLevel DifficultyLevel => GetDifficultyLevel();

    public int Altitude
    {
        get
        {
            return _altitude;
        }
        private set
        {
            if (value <= 0 || value > 3150) throw new ArgumentOutOfRangeException(nameof(Altitude));
            _altitude = value;
        }
    }

    public Region Region
    {
        get
        {
            return _region;
        }
        private set
        {
            if (value == Region.NONE) throw new ArgumentOutOfRangeException(nameof(Region));
            _region = value;
        }
    }

    public static Result<Summit, Error> Create(int altitude, string location, string name, Region region, Guid? id = null)
    {
        Summit summit = new(id ?? Guid.NewGuid());

        try
        {
            summit.Altitude = altitude;
        }
        catch (ArgumentOutOfRangeException)
        {
            return CatalogueErrors.InvalidAltitude;
        }

        summit.Location = location;
        summit.Name = name;
        summit.Region = region;

        return summit;
    }

    internal EmptyResult<Error> SetAltitude(int altitude)
    {
        try
        {
            _altitude = altitude;
        }
        catch (ArgumentOutOfRangeException)
        {
            return CatalogueErrors.InvalidAltitude;
        }

        return EmptyResult<Error>.Success();
    }

    internal EmptyResult<Error> SetLocation(string location)
    {
        Location = location;
        return EmptyResult<Error>.Success();
    }

    internal EmptyResult<Error> SetName(string name)
    {
        Name = name;
        return EmptyResult<Error>.Success();
    }

    internal EmptyResult<Error> SetRegion(Region region)
    {
        try
        {
            Region = region;

        }
        catch (ArgumentOutOfRangeException)
        {
            return CatalogueErrors.RegionNotAvailable;
        }        
        
        return EmptyResult<Error>.Success();
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