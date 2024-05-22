namespace Api.Models.Responses;

// Defineix un record per encapsular la resposta de les estadístiques de l'excursionista
public record RetrieveStatisticsResponse(int ReachedSummits, int PendingSummits);