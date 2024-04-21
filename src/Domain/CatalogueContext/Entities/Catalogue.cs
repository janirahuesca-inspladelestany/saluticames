using Domain.CatalogueContext.ValueObjects;
using System.Diagnostics;

namespace Domain.CatalogueContext.Entities;

public sealed class Catalogue
{
    private readonly ICollection<Summit> _summits = new List<Summit>();

    public Catalogue(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private init; }
    public string Name { get; set; } = null!;
    public IEnumerable<Summit> Summits => _summits;

    public IEnumerable<Summit> AddSummits(IDictionary<Guid, SummitDetails> summits) 
    {
        var newSummits = new List<Summit>();

        foreach (var summit in summits)
        {
            var newSummit = AddSummit(summit.Value, summit.Key);
            newSummits.Add(newSummit);
        }

        return newSummits;
    }

    public IEnumerable<Summit> AddSummits(IEnumerable<SummitDetails> summits)
    {
        var newSummits = new List<Summit>();

        foreach (var summit in summits)
        {
            var newSummit = AddSummit(summit);
            newSummits.Add(newSummit);
        }

        return newSummits;
    }

    public Summit AddSummit(SummitDetails summitDetails, Guid? id = null)
    {
        var summitToAdd = Summit.Create(Id, summitDetails, id);
        _summits.Add(summitToAdd);
        return summitToAdd;
    }

    public IEnumerable<Summit> EditSummits(IDictionary<Guid, SummitDetails> summits)
    {
        var newSummits = new List<Summit>();

        foreach (var summit in summits)
        {
            var newSummit = EditSummit(summit.Key, summit.Value);
            if (newSummit is null) continue;
            newSummits.Add(newSummit);
        }

        return newSummits;
    }

    public Summit? EditSummit(Guid id, SummitDetails summitDetails)
    {
        var summitToEdit = _summits.SingleOrDefault(summit => summit.Id == id);
        if (summitToEdit is null) return null;

        summitToEdit.SummitDetails = new()
        {
            Altitude = summitDetails.Altitude,
            Location = summitDetails.Location,
            Name = summitDetails.Name,
            Region = summitDetails.Region
        };

        return summitToEdit;
    }

    public IEnumerable<Guid> RemoveSummits(IEnumerable<Guid> ids)
    {
        var oldSummitIds = new List<Guid>();

        foreach (var id in ids)
        {
            if (RemoveSummit(id)) oldSummitIds.Add(id);
        }

        return oldSummitIds;
    }

    public bool RemoveSummit(Guid id)
    {
        var summitToRemove = _summits.SingleOrDefault(summit => summit.Id == id);
        if (summitToRemove is null) return false;

        return _summits.Remove(summitToRemove);
    }

    public IEnumerable<Summit> List(OrderType order = OrderType.ASC)
    {
        return order == OrderType.ASC ? _summits.OrderBy(summit => summit.SummitDetails.Name) : _summits.OrderByDescending(summit => summit.SummitDetails.Name);
    }

    public IEnumerable<Summit> Filter<T>(T value, FilterType filter, OrderType order = OrderType.ASC)
    {
        if (filter == FilterType.NONE) return Enumerable.Empty<Summit>();

        var filteredSummits = _summits.Where(summit => filter switch
        {
            FilterType.NAME when value is string name => summit.SummitDetails.Name.Contains(name, StringComparison.OrdinalIgnoreCase),
            FilterType.ALTITUDE when value is int altitude => summit.SummitDetails.Altitude == altitude,
            FilterType.LOCATION when value is string location => summit.SummitDetails.Location.Contains(location, StringComparison.OrdinalIgnoreCase),
            FilterType.REGION when value is string region => summit.SummitDetails.Region.Contains(region, StringComparison.OrdinalIgnoreCase),
            FilterType.DIFICULTY when value is DifficultyLevel difficulty => summit.SummitDetails.Difficulty == difficulty,
            _ => throw new UnreachableException()
        });

        return order == OrderType.ASC
            ? filteredSummits.OrderBy(summit => typeof(SummitDetails).GetProperty(filter.ToString(), System.Reflection.BindingFlags.IgnoreCase))
            : filteredSummits.OrderByDescending(summit => typeof(SummitDetails).GetProperty(filter.ToString(), System.Reflection.BindingFlags.IgnoreCase));
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
}