using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

// Defineix un record per encapsular les dades de la sol·licitud per crear un diari (Name i CatalogueId obligatoris)
public record CreateDiaryRequest([Required] string Name, [Required] Guid CatalogueId);
