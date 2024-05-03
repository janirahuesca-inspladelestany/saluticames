using SharedKernel.Abstractions;

namespace Domain.CatalogueContext.Events;

public sealed record SummitCreatedDomainEvent(Guid SummitId) : IDomainEvent;