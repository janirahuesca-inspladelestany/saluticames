using Application.Challenge.Services;
using Application.Content.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

// Classe estàtica per afegir extensions als serveis de l'aplicació
public static class ServiceCollectionExtensions
{
    // Mètode d'extensió per registrar els serveis de l'aplicació
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registra ICatalogueService amb la seva implementació CatalogueService
        services.AddScoped<ICatalogueService, CatalogueService>();

        // Registra ISummitService amb la seva implementació SummitService
        services.AddScoped<ISummitService, SummitService>();

        // Registra IChallengeService amb la seva implementació ChallengeService
        services.AddScoped<IChallengeService, ChallengeService>();

        // Retorna la col·lecció de serveis per permetre la cadena de mètodes
        return services;
    }
}
