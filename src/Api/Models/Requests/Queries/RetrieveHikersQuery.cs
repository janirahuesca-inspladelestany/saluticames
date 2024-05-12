using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Requests.Queries;

public class RetrieveHikersQuery()
{
    [FromQuery(Name = "hikerId")]
    public string? Id { get; init; }

    [FromQuery(Name = "name")]
    public string? Name { get; init; }

    [FromQuery(Name = "surname")]
    public string? Surname { get; init; }
}

