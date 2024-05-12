using Application.Challenge.Services;
using Application.Content.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICatalogueService, CatalogueService>();
        services.AddScoped<ISummitService, SummitService>();
        services.AddScoped<IChallengeService, ChallengeService>();

        return services;
    }
}
