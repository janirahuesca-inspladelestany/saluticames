using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Requests.Queries;

// Defineix la classe per encapsular els paràmetres de la consulta per recuperar cims
public class RetrieveSummitsQuery() 
{
    // Mapeja el paràmetre de consulta "summitId" al camp Id
    [FromQuery(Name = "summitId")] 
    public Guid? Id { get; init; }

    // Mapeja el paràmetre de consulta "minAltitude" al camp MinAltitude
    [FromQuery(Name = "minAltitude")]
    public int? MinAltitude { get; init; }

    // Mapeja el paràmetre de consulta "maxAltitude" al camp MaxAltitude
    [FromQuery(Name = "maxAltitude")]
    public int? MaxAltitude { get; init; }

    // Mapeja el paràmetre de consulta "name" al camp Name
    [FromQuery(Name = "name")]
    public string? Name { get; init; }

    // Mapeja el paràmetre de consulta "isEssential" al camp IsEssential
    [FromQuery(Name = "isEssential")]
    public bool? IsEssential { get; init; }

    // Mapeja el paràmetre de consulta "region" al camp Region
    [FromQuery(Name = "region")]
    public string? RegionName { get; init; }

    // Mapeja el paràmetre de consulta "difficulty" al camp Difficulty
    [FromQuery(Name = "difficulty")]
    public string? DifficultyLevel { get; init; }
}
