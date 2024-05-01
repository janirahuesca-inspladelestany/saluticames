using SharedKernel.Abstractions;

namespace Application.Abstractions;

public interface IRepository<T, TId>
    where T : AggregateRoot<TId>
{
}
