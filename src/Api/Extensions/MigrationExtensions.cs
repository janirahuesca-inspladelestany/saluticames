using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Api.Extensions;

public static class MigrationExtensions
{
    public async static void ApplyMigrationsAsync(this IApplicationBuilder app, IConfiguration configuration) 
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<SalutICamesDbContext>();

        await dbContext.Database.MigrateAsync();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var user = new IdentityUser
        {
            Id = "1",
            UserName = "admin@saluticames.com",
            Email = "admin@saluticames.com"
        };

        if (!userManager.Users.Any(u => u.Id == user.Id))
        {
            var passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, configuration.GetRequiredSection("Identity")["AdminPassword"]);

            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, "Admin");
        }

    }
}
