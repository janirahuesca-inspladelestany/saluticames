using Domain.CatalogueContext.DTO;
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

    public Guid Id { get; private set; }
    public IEnumerable<Summit> Summits => _summits;

    public void AddSummits(IEnumerable<SummitDto> summitDtos)
    {
        foreach (var summitDto in summitDtos)
        {
            AddSummit(summitDto);
        }
    }

    public void AddSummit(SummitDto summitDto)
    {
        var summitToAdd = Summit.Create(summitDto);
        _summits.Add(summitToAdd);
    }

    public void EditSummits(IDictionary<Guid, SummitDto> summits)
    {
        foreach(var summitDto in summits) 
        {
            EditSummit(summitDto.Key, summitDto.Value);
        }
    }

    public void EditSummit(Guid id, SummitDto summitDto)
    {
        var summitToEdit = _summits.SingleOrDefault(summit => summit.Id == id);
        if (summitToEdit is null) return;

        summitToEdit.Altitude = summitDto.Altitude;
        summitToEdit.Location = summitDto.Location;
        summitToEdit.Name = summitDto.Name;
        summitToEdit.Region = summitDto.Region;
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

    public IEnumerable<Summit> List<T>(T value, FilterType filter = FilterType.NONE, OrderType order = OrderType.ASC)
    {
        var summitsToList = filter is FilterType.NONE ? _summits : _summits.Where(summit => filter switch
        {
            FilterType.NAME when value is string name => summit.Name.Contains(name, StringComparison.OrdinalIgnoreCase),
            FilterType.ALTITUDE when value is int altitude => summit.Altitude == altitude,
            FilterType.LOCATION when value is string location => summit.Location.Contains(location, StringComparison.OrdinalIgnoreCase),
            FilterType.REGION when value is string region => summit.Region.Contains(region, StringComparison.OrdinalIgnoreCase),
            FilterType.DIFICULTY when value is DifficultyLevel difficulty => summit.Difficulty == difficulty,
            _ => throw new UnreachableException()
        });

        return order == OrderType.ASC ? summitsToList.Order() : summitsToList.OrderDescending();
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