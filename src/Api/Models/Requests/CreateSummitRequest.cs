using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

// Defineix un record per encapsular les dades de la sol·licitud per crear un cim (Name, Altitude, Location, IsEssential i RegionName obligatoris)
public record CreateSummitRequest([Required] string Name, [Required] int Altitude, [Required] string Location, [Required] bool IsEssential, [Required] string RegionName);
