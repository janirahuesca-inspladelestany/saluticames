using Application.CatalogueContext.Services;
using Domain.CatalogueContext.Repositories;
using Domain.CatalogueContext.Services;
using Persistence;

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

        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddSingleton<ICatalogueRepository, FakeCatalogueRepository>();

            return services;
        }
    }
}
