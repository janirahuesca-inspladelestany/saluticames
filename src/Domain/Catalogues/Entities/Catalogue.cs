using Domain.Catalogues.Enums;
using SharedKernel.Abstractions;
using SharedKernel.Helpers;

namespace Domain.Catalogues.Entities;

public sealed class Catalogue : AggregateRoot<Guid>
{
    private readonly ICollection<Summit> _summits = new List<Summit>();

    private Catalogue(Guid id)
        : base(id)
    {
    }

    public enum FilterType
    {
        NONE,
        NAME,
        ALTITUDE,
        LOCATION,
        REGION,
        DIFICULTY
    }

    public enum OrderType
    {
        ASC,
        DESC
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

    public void ReplaceSummits(IDictionary<Guid, (int? Altitude, string? Location, string? Name, string? Region)> summitsToReplace)
    {
        foreach (var summit in summitsToReplace)
        {
            ReplaceSummit(summit.Key, (summit.Value.Altitude, summit.Value.Location, summit.Value.Name, summit.Value.Region));
        }
    }

    public void ReplaceSummit(Guid id, (int? Altitude, string? Location, string? Name, string? Region) summitDetailToReplace)
    {
        var summit = _summits.SingleOrDefault(summit => summit.Id == id);
        if (summit is null) return;

        summit.Altitude = summitDetailToReplace.Altitude ?? summit.Altitude;
        summit.Location = summitDetailToReplace.Location ?? summit.Location;
        summit.Name = summitDetailToReplace.Name ?? summit.Name;
        summit.Region = !string.IsNullOrEmpty(summitDetailToReplace.Region) && EnumHelper.IsDefinedByDescription<Region>(summitDetailToReplace.Region)
            ? EnumHelper.GetEnumValueByDescription<Region>(summitDetailToReplace.Region) : summit.Region;
    }

    public void ClearSummits()
    {
        _summits.Clear();
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
}