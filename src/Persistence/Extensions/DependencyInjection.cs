using Application.Abstractions;
using Application.Challenge.Repositories;
using Application.Content.Repositories;
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
        services.AddDbContext<SalutICamesDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SalutICamesDbContext"), providerOptions => providerOptions.EnableRetryOnFailure());
        });

        services.AddScoped<ICatalogueRepository, CatalogueRepository>();
        services.AddScoped<ISummitRepository, SummitRepository>();
        services.AddScoped<IHikerRepository, HikerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
