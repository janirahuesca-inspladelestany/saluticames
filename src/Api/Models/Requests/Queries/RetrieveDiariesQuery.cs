using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Requests.Queries;

// Defineix la classe per encapsular els paràmetres de la consulta per recuperar diaris
public class RetrieveDiariesQuery()
{
    // Mapeja el paràmetre de consulta "diaryId" al camp Id
    [FromQuery(Name = "diaryId")]
    public Guid? Id { get; init; }

    // Mapeja el paràmetre de consulta "name" al camp Name
    [FromQuery(Name = "name")]
    public string? Name { get; init; }

    // Mapeja el paràmetre de consulta "hikerId" al camp HikerId
    [FromQuery(Name = "hikerId")]
    public string? HikerId { get; init; }
}

