namespace Api.Models.Responses;

// Defineix un record per encapsular la resposta de l'ascensió
public record RetrieveClimbResponse(Guid SummitId, DateTime AscensionDate);
