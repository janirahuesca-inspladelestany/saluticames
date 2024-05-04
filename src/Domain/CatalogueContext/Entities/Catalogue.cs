using Domain.CatalogueContext.Enums;
using Domain.CatalogueContext.Errors;
using SharedKernel.Abstractions;
using SharedKernel.Common;
using SharedKernel.Helpers;

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

    public static Catalogue Create(string name)
    {
        return new Catalogue(Guid.NewGuid())
        {
            Name = name
        };
    }

    public void AddSummits(IEnumerable<Summit> summitsToAdd)
    {
        foreach (var summit in summitsToAdd)
        {
            AddSummit(summit);
        }
    }

    public void AddSummit(Summit summitToAdd)
    {
        _summits.Add(summitToAdd);
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
        if (summit is null) return EmptyResult<Error>.Success();

        if (summitDetailToReplace.Altitude.HasValue) 
        {
            var setAltitudeResult = summit.SetAltitude(summitDetailToReplace.Altitude.Value);
            if (setAltitudeResult.IsFailure()) return setAltitudeResult.Error;
        }

        if (!string.IsNullOrEmpty(summitDetailToReplace.Location))
        {
            var setLocationResult = summit.SetLocation(summitDetailToReplace.Location);
            if (setLocationResult.IsFailure()) return setLocationResult.Error;
        }

        if (!string.IsNullOrEmpty(summitDetailToReplace.Name))
        {
            var setNameResult = summit.SetName(summitDetailToReplace.Name);
            if (setNameResult.IsFailure()) return setNameResult.Error;
        }

        if (!string.IsNullOrEmpty(summitDetailToReplace.RegionName))
        {
            if (!EnumHelper.IsDefinedByDescription<Region>(summitDetailToReplace.RegionName)) return CatalogueErrors.RegionNotAvailable;
            var setRegionResult = summit.SetRegion(EnumHelper.GetEnumValueByDescription<Region>(summitDetailToReplace.RegionName));
            if (setRegionResult.IsFailure()) return setRegionResult.Error;
        }

        return EmptyResult<Error>.Success();
    }

    public IEnumerable<Summit> RemoveSummits(IEnumerable<Guid> summitIdsToRemove)
    {
        var summits = new List<Summit>();

        foreach (var summitId in summitIdsToRemove)
        {
            if (RemoveSummit(summitId, out var summit))
            {
                summits.Add(summit!);
            }
        }

        return summits;
    }

    public bool RemoveSummit(Guid summitIdToRemove, out Summit? summit)
    {
        summit = _summits.SingleOrDefault(summit => summit.Id == summitIdToRemove);
        if (summit is null) return false;

        return _summits.Remove(summit);
    }

    public record SummitDetail(int? Altitude, string? Location, string? Name, string? RegionName);
}