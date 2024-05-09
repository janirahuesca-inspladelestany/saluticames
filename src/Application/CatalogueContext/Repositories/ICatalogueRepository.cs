using Application.Abstractions;
using Domain.CatalogueContext.Entities;
using System.Linq.Expressions;

namespace Application.CatalogueContext.Repositories;

public interface ICatalogueRepository : IRepository<Catalogue, Guid>
{
    Task<IEnumerable<Catalogue>> ListAsync(Expression<Func<Catalogue, bool>>? filter = null,
        Func<IQueryable<Catalogue>, IOrderedQueryable<Catalogue>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default);
    Task<Catalogue?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Summit>> GetSummitsAsync(Guid id,
        Expression<Func<Summit, bool>>? filter = null,
        Func<IQueryable<Summit>, IOrderedQueryable<Summit>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default);
}
