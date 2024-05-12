namespace SharedKernel.Abstractions;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected Entity(TId id) 
    {
        Id = id;
    }

    public TId Id { get; init; } = default!;
    public List<IDomainEvent> DomainEvents => _domainEvents.ToList();

    public bool Equals(Entity<TId>? other)
    {
        return other is not null && other.Id.Equals(Id);
    }

    protected void Raise(IDomainEvent domainEvent) 
    {
        _domainEvents.Add(domainEvent);
    }
}
