using SharedKernel.Abstractions;

namespace Domain.Catalogues.Events;

public sealed record SummitCreatedDomainEvent(Guid SummitId) : IDomainEvent;