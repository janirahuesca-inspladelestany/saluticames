using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.OpenApi;

public class ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider _provider) : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var openApiInfo = new OpenApiInfo()
            {
                Title = $"SalutICames.Api v{description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            };

            options.SwaggerDoc(description.GroupName, openApiInfo);
        }
    }
}
