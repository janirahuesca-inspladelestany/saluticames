using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Requests.Queries;

// Defineix la classe per encapsular els paràmetres de la consulta per recuperar excursionites
public class RetrieveHikersQuery()
{
    // Mapeja el paràmetre de consulta "hikerId" al camp Id
    [FromQuery(Name = "hikerId")]
    public string? Id { get; init; }

    // Mapeja el paràmetre de consulta "name" al camp Name
    [FromQuery(Name = "name")]
    public string? Name { get; init; }

    // Mapeja el paràmetre de consulta "surname" al camp Surname
    [FromQuery(Name = "surname")]
    public string? Surname { get; init; }
}

