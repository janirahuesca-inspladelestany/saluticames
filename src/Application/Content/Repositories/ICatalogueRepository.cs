using Application.Abstractions;
using Domain.Content.Entities;
using System.Linq.Expressions;

namespace Application.Content.Repositories;

public interface ICatalogueRepository : IRepository<CatalogueAggregate, Guid>
{
    Task<IEnumerable<CatalogueAggregate>> ListAsync(Expression<Func<CatalogueAggregate, bool>>? filter = null,
        Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default);
    Task<CatalogueAggregate?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
