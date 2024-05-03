using Application.CatalogueContext.Services;
using Application.ChallengeContext.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICatalogueService, CatalogueService>();
        services.AddScoped<IChallengeService, ChallengeService>();

        return services;
    }
}
