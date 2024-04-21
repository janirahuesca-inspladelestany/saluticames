
using Microsoft.EntityFrameworkCore;

namespace Persistence.CatalogueContext.Data;

public class DbInitializerHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DbInitializerHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await PopulateDatabase();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task PopulateDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var catalogueDbContext = scope.ServiceProvider.GetRequiredService<CatalogueDbContext>();
        await catalogueDbContext.Database.MigrateAsync();
    }
}
