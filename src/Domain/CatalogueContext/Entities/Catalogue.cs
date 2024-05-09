using Domain.CatalogueContext.Enums;
using Domain.CatalogueContext.Errors;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.CatalogueContext.Entities;

public sealed class Catalogue : AggregateRoot<Guid>
{
    private readonly ICollection<Summit> _summits = new List<Summit>();

    private Catalogue(Guid id)
        : base(id)
    {
    }

    public string Name { get; private set; } = null!;
    public IEnumerable<Summit> Summits => _summits;

    public static Catalogue Create(string name, Guid? id = null)
    {
        return new Catalogue(id ?? Guid.NewGuid())
        {
            Name = name
        };
    }

    public Result<IEnumerable<Guid>, Error> AddSummits(IEnumerable<Summit> summitsToAdd)
    {
        var summits = new List<Guid>();

        foreach (var summit in summitsToAdd)
        {
            var addSummitResult = AddSummit(summit);
            if (addSummitResult.IsFailure()) return addSummitResult.Error;
            summits.Add(summit!.Id);
        }

        return summits;
    }

    public EmptyResult<Error> AddSummit(Summit summitToAdd)
    {
        if (DoesSummitNameExistInCatalogue(summitToAdd.Name))
        {
            return CatalogueErrors.SummitNameAlreadyExists;
        }

        _summits.Add(summitToAdd);

        return EmptyResult<Error>.Success();
    }

    public Result<IEnumerable<Guid>, Error> ReplaceSummits(IDictionary<Guid, SummitDetail> summitsToReplace)
    {
        var summits = new List<Guid>();

        foreach (var summitToReplace in summitsToReplace)
        {
            var replaceSummitResult = ReplaceSummit(summitToReplace.Key, summitToReplace.Value, out var summit);
            if (replaceSummitResult.IsFailure()) return replaceSummitResult.Error;
            summits.Add(summit!.Id);
        }

        return summits;
    }

    public EmptyResult<Error> ReplaceSummit(Guid id, SummitDetail summitDetailToReplace, out Summit? summit)
    {
        summit = _summits.SingleOrDefault(summit => summit.Id == id);

        if (summit is null) return CatalogueErrors.SummitIdNotFound;

        if (!string.IsNullOrEmpty(summitDetailToReplace.Name))
        {
            if (DoesSummitNameExistInCatalogue(summitDetailToReplace.Name))
            {
                return CatalogueErrors.SummitIdNotFound;
            }

            var setNameResult = summit.SetName(summitDetailToReplace.Name);
            if (setNameResult.IsFailure()) return setNameResult.Error;
        }

        if (summitDetailToReplace.Altitude.HasValue)
        {
            var setAltitudeResult = summit.SetAltitude(summitDetailToReplace.Altitude.Value);
            if (setAltitudeResult.IsFailure()) return setAltitudeResult.Error;
        }

        if (summitDetailToReplace.Latitude.HasValue)
        {
            var setLatitudeResult = summit.SetLatitude(summitDetailToReplace.Latitude.Value);
            if (setLatitudeResult.IsFailure()) return setLatitudeResult.Error;
        }

        if (summitDetailToReplace.Longitude.HasValue)
        {
            var setLongitudeResult = summit.SetLongitude(summitDetailToReplace.Longitude.Value);
            if (setLongitudeResult.IsFailure()) return setLongitudeResult.Error;
        }

        if (summitDetailToReplace.IsEssential.HasValue)
        {
            var setIsEssentialResult = summit.SetIsEssential(summitDetailToReplace.IsEssential.Value);
            if (setIsEssentialResult.IsFailure()) return setIsEssentialResult.Error;
        }

        if (summitDetailToReplace.Region.HasValue)
        {
            var setRegionResult = summit.SetRegion(summitDetailToReplace.Region.Value);
            if (setRegionResult.IsFailure()) return setRegionResult.Error;
        }

        return EmptyResult<Error>.Success();
    }

    public Result<IEnumerable<Summit>, Error> RemoveSummits(IEnumerable<Guid> summitIdsToRemove)
    {
        var summits = new List<Summit>();

        foreach (var summitId in summitIdsToRemove)
        {
            var summitRemoveResult = RemoveSummit(summitId, out var summit);
            if (summitRemoveResult.IsFailure()) return summitRemoveResult.Error;
            
            summits.Add(summit!);
        }

        return summits;
    }

    public EmptyResult<Error> RemoveSummit(Guid summitIdToRemove, out Summit? summit)
    {
        summit = _summits.SingleOrDefault(summit => summit.Id == summitIdToRemove);
        if (summit is null) return CatalogueErrors.SummitIdNotFound;

        return _summits.Remove(summit) ? EmptyResult<Error>.Success() : CatalogueErrors.SummitRemoveFailure;
    }

    private bool DoesSummitNameExistInCatalogue(string summitName)
    {
        return _summits.Any(summit =>
            summit.Name.Equals(summitName, StringComparison.InvariantCultureIgnoreCase));
    }

    public record SummitDetail(string? Name, int? Altitude, float? Latitude, float? Longitude, bool? IsEssential, Region? Region);
}