using Domain.CatalogueContext.Enums;
using Domain.CatalogueContext.Errors;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.CatalogueContext.Entities;

public sealed class Summit : Entity<Guid>
{
    private Summit(Guid id)
        : base(id)
    {
        DifficultyLevel = CalculateDifficultyLevel();
    }

    public string Name { get; private set; } = null!;
    public int Altitude { get; private set; }
    public string Latitude { get; private set; } = null!;
    public string Longitude { get; private set; } = null!;
    public bool IsEssential { get; private set; }
    public Region Region { get; private set; }
    public DifficultyLevel DifficultyLevel { get; private set; }
    public Guid CatalogueId { get; private set; }

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
        if (altitude <= 0 || altitude > 3150)
        {
            return CatalogueErrors.SummitInvalidAltitude;
        }

        Altitude = altitude;
        DifficultyLevel = CalculateDifficultyLevel();

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
        if (region == Region.NONE)
        {
            return CatalogueErrors.SummitRegionNotAvailable;
        }

        Region = region;

        return EmptyResult<Error>.Success();
    }

    private DifficultyLevel CalculateDifficultyLevel()
    {
        return Altitude switch
        {
            < 1500 => DifficultyLevel.EASY,
            >= 1500 and < 2500 => DifficultyLevel.MODERATE,
            >= 2500 => DifficultyLevel.DIFFICULT
        };
    }
}