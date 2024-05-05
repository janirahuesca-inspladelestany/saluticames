using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Requests.Queries;

public class ReadSummitsQuery() 
{
    [FromQuery(Name = "summitId")] 
    public Guid? Id { get; init; }

    [FromQuery(Name = "minAltitude")]
    public int? MinAltitude { get; init; }

    [FromQuery(Name = "maxAltitude")]
    public int? MaxAltitude { get; init; }

    [FromQuery(Name = "name")]
    public string? Name { get; init; }

    [FromQuery(Name = "isEssential")]
    public bool? IsEssential { get; init; }

    [FromQuery(Name = "region")]
    public string? RegionName { get; init; }
}
