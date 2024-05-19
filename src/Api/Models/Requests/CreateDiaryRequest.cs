using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

public record CreateDiaryRequest([Required] string Name, [Required] Guid CatalogueId);
