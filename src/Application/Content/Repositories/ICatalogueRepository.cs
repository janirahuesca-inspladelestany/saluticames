using Application.Abstractions;
using Domain.Content.Entities;
using System.Linq.Expressions;

namespace Application.Content.Repositories;

public interface ICatalogueRepository : IRepository<Catalogue, Guid>
{
    Task<IEnumerable<Catalogue>> ListAsync(Expression<Func<Catalogue, bool>>? filter = null,
        Func<IQueryable<Catalogue>, IOrderedQueryable<Catalogue>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default);
    Task<Catalogue?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
