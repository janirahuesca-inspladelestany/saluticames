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

    public string Name { get; private set; } = null!;
    public string Latitude { get; private set; } = null!;
    public string Longitude { get; private set; } = null!;
    public bool IsEssential { get; private set; }
    public DifficultyLevel DifficultyLevel => GetDifficultyLevel();
    public Guid CatalogueId { get; private set; }

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

    public static Result<Summit, Error> Create(string name, int altitude, string latitude, string longitude, bool isEssential, Region region, Guid? id = null)
    {
        Summit summit = new(id ?? Guid.NewGuid());

        try
        {
            summit.Altitude = altitude;
        }
        catch (ArgumentOutOfRangeException)
        {
            return CatalogueErrors.SummitInvalidAltitude;
        }

        summit.Name = name;
        summit.Latitude = latitude;
        summit.Longitude = longitude;
        summit.IsEssential = isEssential;
        summit.Region = region;

        return summit;
    }

    internal EmptyResult<Error> SetName(string name)
    {
        Name = name;
        return EmptyResult<Error>.Success();
    }

    internal EmptyResult<Error> SetAltitude(int altitude)
    {
        try
        {
            _altitude = altitude;
        }
        catch (ArgumentOutOfRangeException)
        {
            return CatalogueErrors.SummitInvalidAltitude;
        }

        return EmptyResult<Error>.Success();
    }

    internal EmptyResult<Error> SetLatitude(string latitude)
    {
        Latitude = latitude;
        return EmptyResult<Error>.Success();
    }

    internal EmptyResult<Error> SetLongitude(string longitude)
    {
        Longitude = longitude;
        return EmptyResult<Error>.Success();
    }

    internal EmptyResult<Error> SetIsEssential(bool isEssential)
    {
        IsEssential = isEssential;
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
            return CatalogueErrors.SummitRegionNotAvailable;
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