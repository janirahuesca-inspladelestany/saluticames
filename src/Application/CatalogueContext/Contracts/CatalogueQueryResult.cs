namespace Application.CatalogueContext.Contracts;

public record CatalogueQueryResult(Guid Id, IEnumerable<SummitQueryResult> Summits);
