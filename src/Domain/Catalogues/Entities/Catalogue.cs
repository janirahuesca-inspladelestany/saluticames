using Domain.Catalogues.Enums;
using SharedKernel.Abstractions;
using System.Diagnostics;

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

    public void ReplaceSummits(IEnumerable<Summit> summitsToReplace)
    {
        foreach (var summit in summitsToReplace)
        {
            ReplaceSummit(summit);
        }
    }

    public void ReplaceSummit(Summit summitToReplace)
    {
        var summit = _summits.SingleOrDefault(summit => summit.Id == summitToReplace.Id);
        if (summit is null) return;

        summit.Altitude = summitToReplace.Altitude;
        summit.Location = summitToReplace.Location;
        summit.Name = summitToReplace.Name;
        summit.Region = summitToReplace.Region;
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
        if(summit is null) return false;

        return _summits.Remove(summit);
    }

    public IEnumerable<Summit> GetSummits(OrderType order = OrderType.ASC)
    {
        return order == OrderType.ASC ? _summits.OrderBy(summit => summit.Name) : _summits.OrderByDescending(summit => summit.Name);
    }

    public IEnumerable<Summit> FilterSummitsBy<T>(T value, FilterType filter, OrderType order = OrderType.ASC)
    {
        if (filter == FilterType.NONE) return Enumerable.Empty<Summit>();

        var filteredSummits = _summits.Where(summit => filter switch
        {
            FilterType.NAME when value is string name => summit.Name.Contains(name, StringComparison.OrdinalIgnoreCase),
            FilterType.ALTITUDE when value is int altitude => summit.Altitude == altitude,
            FilterType.LOCATION when value is string location => summit.Location.Contains(location, StringComparison.OrdinalIgnoreCase),
            //FilterType.REGION when value is string region => summit.Region.Name.Contains(region, StringComparison.OrdinalIgnoreCase),
            FilterType.DIFICULTY when value is DifficultyLevel difficulty => summit.DifficultyLevel == difficulty,
            _ => throw new UnreachableException()
        });

        return order == OrderType.ASC
            ? filteredSummits.OrderBy(summit => typeof(Summit).GetProperty(filter.ToString(), System.Reflection.BindingFlags.IgnoreCase))
            : filteredSummits.OrderByDescending(summit => typeof(Summit).GetProperty(filter.ToString(), System.Reflection.BindingFlags.IgnoreCase));
    }
}