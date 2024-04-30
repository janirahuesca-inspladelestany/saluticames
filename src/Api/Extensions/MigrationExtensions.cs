using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app) 
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<CatalogueDbContext>();

        dbContext.Database.Migrate();
    }
}
