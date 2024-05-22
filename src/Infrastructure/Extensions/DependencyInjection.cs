using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;

namespace Infrastructure.Extensions;

public static class DependencyInjection
{
    // Mètode per afegir serveis d'infraestructura al contenidor de dependències
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Afegir comprovacions de salut
        services.AddHealthChecks()
            .AddDbContextCheck<SalutICamesDbContext>(); // Afegir una comprovació per a la base de dades

        return services; // Retorna la col·lecció de serveis
    }
}
