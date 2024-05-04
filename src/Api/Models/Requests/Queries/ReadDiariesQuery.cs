using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Requests.Queries;

public class ReadDiariesQuery()
{
    [FromQuery(Name = "diaryId")]
    public Guid? Id { get; init; }
    [FromQuery(Name = "name")]
    public string? Name { get; init; }
    [FromQuery(Name = "hikerId")]
    public string? HikerId { get; init; }
}

