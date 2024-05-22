using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

// Defineix un record per encapsular les dades de la sol·licitud per crear una ascensió (summitId obligatori, ascensionDateTime opcional)
public record CreateClimbRequest([Required] Guid summitId, DateTime? ascensionDateTime);
