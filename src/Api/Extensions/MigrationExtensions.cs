using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Api.Extensions;

public static class MigrationExtensions
{
    // Mètode d'extensió per aplicar migracions i crear un usuari administrador
    public async static void ApplyMigrationsAsync(this IApplicationBuilder app, IConfiguration configuration) 
    {
        // Crear un scope per accedir als serveis registrats a l'IoC
        using var scope = app.ApplicationServices.CreateScope();

        // Obtenir una instància del DbContext
        using var dbContext = scope.ServiceProvider.GetRequiredService<SalutICamesDbContext>();

        /// Aplicar les migracions pendents a la base de dades
        await dbContext.Database.MigrateAsync();

        // Obtenir una instància del UserManager per gestionar els usuaris
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Definir un nou usuari d'identitat amb les propietats especificades
        var user = new IdentityUser
        {
            Id = "1",
            UserName = "admin@saluticames.com",
            Email = "admin@saluticames.com"
        };

        // Comprovar si l'usuari amb l'Id especificat ja existeix a la base de dades
        if (!userManager.Users.Any(u => u.Id == user.Id))
        {
            // Generar un hash de contrasenya per l'usuari
            var passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, configuration.GetRequiredSection("Identity")["AdminPassword"]);

            // Crear l'usuari a la base de dades
            await userManager.CreateAsync(user);

            // Assignar el rol d'administrador a l'usuari creat
            await userManager.AddToRoleAsync(user, "Admin");
        }

    }
}
