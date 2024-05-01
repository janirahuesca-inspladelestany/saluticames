using Application.Abstractions;
using Application.Catalogues.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Repositories;

namespace Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogueDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("CatalogueDbContext"), providerOptions => providerOptions.EnableRetryOnFailure());
        });

        services.AddScoped<ICatalogueRepository, CatalogueRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
