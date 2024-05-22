using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;

namespace Api.Models.Requests.Queries;

// Defineix la classe per encapsular els paràmetres de la consulta per recuperar catàlegs
public class RetrieveCataloguesQuery()
{
    // Mapeja el paràmetre de consulta "catalogueId" al camp Id
    [FromQuery(Name = "catalogueId")]
    public Guid? Id { get; init; }

    // Mapeja el paràmetre de consulta "name" al camp Name
    [FromQuery(Name = "name")]
    public string? Name { get; init; }
}

