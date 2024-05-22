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
    // Mètode d'extensió per afegir els serveis de persistència al contenidor d'injecció de dependències
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar el context de la base de dades amb Entity Framework Core
        services.AddDbContext<SalutICamesDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SalutICamesDbContext"), providerOptions => providerOptions.EnableRetryOnFailure());
        });

        // Registrar els repositoris i la unitat de treball
        services.AddScoped<ICatalogueRepository, CatalogueRepository>();
        services.AddScoped<ISummitRepository, SummitRepository>();
        services.AddScoped<IHikerRepository, HikerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
