using Domain.Content.Enums;
using Domain.Content.Errors;
using Domain.Content.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.Content.Entities;

public sealed class Summit : AggregateRoot<Guid>
{
    internal readonly List<CatalogueSummit> _catalogueSummit = new List<CatalogueSummit>();

    private Summit(Guid id)
        : base(id)
    {
        DifficultyLevel = CalculateDifficultyLevel();
    }

    public string Name { get; private set; } = null!;
    public int Altitude { get; private set; }
    public float Latitude { get; private set; }
    public float Longitude { get; private set; }
    public bool IsEssential { get; private set; }
    public Region Region { get; private set; }
    public DifficultyLevel DifficultyLevel { get; private set; }
    public IEnumerable<Guid> CatalogueIds => _catalogueSummit.Select(catalogueSummit => catalogueSummit.CatalogueId);
    public IReadOnlyCollection<CatalogueSummit> CatalogueSummits => _catalogueSummit; // Navigation property

    public static Result<Summit?, Error> Create(string name, int altitude, float latitude, float longitude, bool isEssential, Region region, Guid? id = null)
    {
        Summit summit = new(id ?? Guid.NewGuid());

        var setNameResult = summit.SetName(name);
        if (setNameResult.IsFailure()) return setNameResult.Error;

        var setAltitudeResult = summit.SetAltitude(altitude);
        if (setAltitudeResult.IsFailure()) return setAltitudeResult.Error;

        var setLatitudeResult = summit.SetLatitude(latitude);
        if (setLatitudeResult.IsFailure()) return setLatitudeResult.Error;

        var setLongitudeResult = summit.SetLongitude(longitude);
        if (setLongitudeResult.IsFailure()) return setLongitudeResult.Error;

        var setIsEssentialResult = summit.SetIsEssential(isEssential);
        if (setIsEssentialResult.IsFailure()) return setIsEssentialResult.Error;

        var setRegionResult = summit.SetRegion(region);
        if (setRegionResult.IsFailure()) return setRegionResult.Error;

        return summit;
    }

    public EmptyResult<Error> SetName(string name)
    {
        Name = name;
        return EmptyResult<Error>.Success();
    }

    public EmptyResult<Error> SetAltitude(int altitude)
    {
        if (altitude <= 0 || altitude > 3150)
        {
            return SummitErrors.SummitInvalidAltitude;
        }

        Altitude = altitude;
        DifficultyLevel = CalculateDifficultyLevel();

        return EmptyResult<Error>.Success();
    }

    public EmptyResult<Error> SetLatitude(float latitude)
    {
        Latitude = latitude;
        return EmptyResult<Error>.Success();
    }

    public EmptyResult<Error> SetLongitude(float longitude)
    {
        Longitude = longitude;
        return EmptyResult<Error>.Success();
    }

    public EmptyResult<Error> SetIsEssential(bool isEssential)
    {
        IsEssential = isEssential;
        return EmptyResult<Error>.Success();
    }

    public EmptyResult<Error> SetRegion(Region region)
    {
        if (region == Region.None)
        {
            return SummitErrors.SummitRegionNotAvailable;
        }

        Region = region;

        return EmptyResult<Error>.Success();
    }

    private DifficultyLevel CalculateDifficultyLevel()
    {
        return Altitude switch
        {
            < 1500 => DifficultyLevel.Easy,
            >= 1500 and < 2500 => DifficultyLevel.Moderate,
            >= 2500 => DifficultyLevel.Difficult
        };
    }
}