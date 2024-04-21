using Application.CatalogueContext.Services;
using Domain.CatalogueContext.Repositories;
using Domain.CatalogueContext.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.CatalogueContext.Data;
using Persistence.CatalogueContext.Repositories;

namespace Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services) 
        {
            services.AddScoped<ICatalogueService, CatalogueService>();
            services.AddScoped<IRegionService, RegionService>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogueDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"), providerOptions => providerOptions.EnableRetryOnFailure());
            }).AddHostedService<DbInitializerHostedService>();

            services.AddScoped<ICatalogueRepository, CatalogueRepository>();

            return services;
        }
    }
}
