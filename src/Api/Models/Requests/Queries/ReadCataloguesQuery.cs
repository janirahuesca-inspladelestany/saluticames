using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Requests.Queries;

public class ReadCataloguesQuery()
{
    [FromQuery(Name = "catalogueId")]
    public Guid? Id { get; init; }
    [FromQuery(Name = "name")]
    public string? Name { get; init; }
}

