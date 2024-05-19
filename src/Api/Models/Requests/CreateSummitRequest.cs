using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

public record CreateSummitRequest([Required] string Name, [Required] int Altitude, [Required] string Location, [Required] bool IsEssential, [Required] string RegionName);
