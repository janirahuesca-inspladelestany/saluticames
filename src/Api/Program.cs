using Api.Extensions;
using Api.Infrastructure;
using Api.OpenApi;
using Application.Extensions;
using Asp.Versioning;
using HealthChecks.UI.Client;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Persistence.Data;
using Persistence.Extensions;
using Swashbuckle.AspNetCore.Filters;

// Configuraci� i construcci� de l'aplicaci� ASP.NET Core

// Crear l'aplicaci� web utilitzant el builder
var builder = WebApplication.CreateBuilder(args);

// Afegir serveis al contenidor de depend�ncies
builder.Services.AddAuthorization();

// Configurar l'API d'identitat amb els serveis d'Identity i Entity Framework
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SalutICamesDbContext>();

// Afegir controladors a l'aplicaci�
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Afegir el generador d'API Explorer per a les rutes d'API
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    // Afegir definici� de seguretat per a l'autenticaci� OAuth2
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    // Afegir filtre d'operacions per a requeriments de seguretat
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Afegir serveis d'aplicaci�, infraestructura i persist�ncia
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration);

// Afegir gestor d'excepcions global i suport per a ProblemDetails
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Configurar la versi� de l'API i l'explorador d'API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
}).AddMvc();

// Configurar opcions per a la generaci� de Swagger
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

// Construir l'aplicaci�
var app = builder.Build();

// Configurar el pipeline de les peticions HTTP
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger i Swagger UI en mode de desenvolupament
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
        }
    });

    // Aplicar migracions de base de dades en mode de desenvolupament
    app.ApplyMigrationsAsync(builder.Configuration);
}

// Redireccionar les peticions HTTP a HTTPS
app.UseHttpsRedirection();

// Mapeja les rutes de l'API d'identitat
app.MapIdentityApi<IdentityUser>();

// Configurar la comprovaci� d'estat de salut
app.MapHealthChecks("health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Habilitar l'autenticaci� i autoritzaci�
app.UseAuthentication();

app.UseAuthorization();

// Mapeja les rutes dels controladors
app.MapControllers();

// Habilitar el gestor d'excepcions global
app.UseExceptionHandler();

// Executar l'aplicaci�
app.Run();
