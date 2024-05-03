namespace SharedKernel.Abstractions;

public abstract class Entity<TId>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected Entity(TId id) 
    {
        Id = id;
    }

    public TId Id { get; init; } = default!;
    public List<IDomainEvent> DomainEvents => _domainEvents.ToList();
    protected void Raise(IDomainEvent domainEvent) 
    {
        _domainEvents.Add(domainEvent);
    }
}
