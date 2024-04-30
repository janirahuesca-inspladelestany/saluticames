namespace SharedKernel.Abstractions;

public interface IRepository<T, TId>
    where T : AggregateRoot<TId>
{
}
