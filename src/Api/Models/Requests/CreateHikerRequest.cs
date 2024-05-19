using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

public record CreateHikerRequest([Required] string Id, [Required] string Name, [Required] string Surname);
