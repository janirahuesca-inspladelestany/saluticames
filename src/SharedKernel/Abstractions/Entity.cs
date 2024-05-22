namespace SharedKernel.Abstractions;

// Definició de l'espai de noms SharedKernel.Abstractions, on es troben les abstraccions compartides del sistema
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull // Restricció que assegura que TId no pot ser null
{
    // Llista privada per emmagatzemar els esdeveniments de domini associats a l'entitat
    private readonly List<IDomainEvent> _domainEvents = new();

    // Constructor protegit que inicialitza la propietat Id
    protected Entity(TId id)
    {
        Id = id;
    }

    // Propietat pública i immutable per obtenir l'identificador de l'entitat
    public TId Id { get; init; } = default!;

    // Propietat pública que retorna una còpia de la llista d'esdeveniments de domini
    public List<IDomainEvent> DomainEvents => _domainEvents.ToList();

    // Implementació del mètode Equals de la interfície IEquatable per comparar entitats
    public bool Equals(Entity<TId>? other)
    {
        // Retorna true si l'altre objecte no és null i té el mateix Id
        return other is not null && other.Id.Equals(Id);
    }

    // Mètode protegit per afegir un esdeveniment de domini a la llista
    protected void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
