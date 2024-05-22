using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

// Defineix un record per encapsular les dades de la sol·licitud per crear un excursionista(Id, Name i Surname obligatoris)
public record CreateHikerRequest([Required] string Id, [Required] string Name, [Required] string Surname);
