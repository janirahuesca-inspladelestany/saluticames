using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.OpenApi;

// Classe per configurar les opcions de SwaggerGen utilitzant les descripcions de les versions de l'API
public class ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider _provider) : IConfigureNamedOptions<SwaggerGenOptions>
{
    // Mètode per configurar les opcions de SwaggerGen amb un nom específic
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options); // Deleguem a l'altre mètode Configure per evitar duplicació de codi
    }

    // Mètode per configurar les opcions de SwaggerGen
    public void Configure(SwaggerGenOptions options)
    {

        // Recorrem totes les descripcions de les versions de l'API
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            // Creem una instància de OpenApiInfo amb el títol i la versió de l'API
            var openApiInfo = new OpenApiInfo()
            {
                Title = $"SalutICames.Api v{description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            };

            // Afegim la documentació de Swagger per a cada versió de l'API
            options.SwaggerDoc(description.GroupName, openApiInfo);
        }
    }
}
