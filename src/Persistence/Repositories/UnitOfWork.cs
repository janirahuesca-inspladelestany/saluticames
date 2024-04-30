using Domain.Catalogues.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using SharedKernel.Abstractions;

namespace Persistence.Repositories;

internal sealed class UnitOfWork(CatalogueDbContext catalogueDbContext) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        catalogueDbContext.ChangeTracker.Entries()
            .Where(e => e.Entity is Region).ToList()
            .ForEach(e => e.State = EntityState.Detached);
        
        return catalogueDbContext.SaveChangesAsync(cancellationToken);
    }
}
