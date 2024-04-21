namespace Application.CatalogueContext.Contracts;

public record CatalogueQueryResult(Guid Id, string Name, IEnumerable<SummitQueryResult> Summits);
