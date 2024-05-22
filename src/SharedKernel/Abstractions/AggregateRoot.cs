namespace SharedKernel.Abstractions;

// Definició de l'espai de noms SharedKernel.Abstractions, on es troben les abstraccions compartides del sistema
public abstract class AggregateRoot<TId> : Entity<TId>
{
    // Constructor protegit que inicialitza la propietat Id de la classe base Entity
    protected AggregateRoot(TId id)
        : base(id)
    {
        // El constructor crida al constructor de la classe base Entity passant el valor id
    }
}
