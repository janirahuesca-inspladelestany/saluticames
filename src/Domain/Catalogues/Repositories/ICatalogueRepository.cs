using Domain.Catalogues.Entities;
using SharedKernel.Abstractions;

namespace Domain.Catalogues.Repositories;

public interface ICatalogueRepository : IRepository<Catalogue, Guid>
{
    Task<Catalogue?> GetByIdWithSummitsAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Catalogue>> ListAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Summit>> ListSummitsAsync(CancellationToken cancellationToken);
    Task AddSummitRangeAsync(IEnumerable<Summit> summits, CancellationToken cancellationToken);
    void ReplaceSummitRange(IEnumerable<Summit> summits);
    void RemoveSummitRange(IEnumerable<Summit> summits);
}
