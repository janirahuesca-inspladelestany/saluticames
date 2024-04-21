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
    public IEnumerable<Summit> Summits => _summits;

    public void AddSummits(IEnumerable<SummitDetails> summitDetailss)
    {
        foreach (var summitDetails in summitDetailss)
        {
            AddSummit(summitDetails);
        }
    }

    public void AddSummit(SummitDetails summitDetails)
    {
        var summitToAdd = Summit.Create(summitDetails);
        _summits.Add(summitToAdd);
    }

    public void EditSummits(IDictionary<Guid, SummitDetails> summits)
    {
        foreach (var summitDetails in summits)
        {
            EditSummit(summitDetails.Key, summitDetails.Value);
        }
    }

    public void EditSummit(Guid id, SummitDetails summitDetails)
    {
        var summitToEdit = _summits.SingleOrDefault(summit => summit.Id == id);
        if (summitToEdit is null) return;

        summitToEdit.SummitDetails = new()
        {
            Altitude = summitDetails.Altitude,
            Location = summitDetails.Location,
            Name = summitDetails.Name,
            Region = summitDetails.Region
        };
    }

    public void RemoveSummits(IEnumerable<Guid> ids)
    {
        foreach (var id in ids)
        {
            RemoveSummit(id);
        }
    }

    public void RemoveSummit(Guid id)
    {
        var summitToRemove = _summits.SingleOrDefault(summit => summit.Id == id);
        if (summitToRemove is null) return;

        _summits.Remove(summitToRemove);
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