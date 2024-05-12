using SharedKernel.Abstractions;

namespace Domain.Content.Events;

public sealed record SummitCreatedDomainEvent(Guid SummitId) : IDomainEvent;