using Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;
using Lab13_JhuniorGutierrez.Domain.Constants;
using Lab13_JhuniorGutierrez.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lab13_JhuniorGutierrez.Infrastructure.Data;


public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<Lab13_JhuniorGutierrezDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(DbSeeder));

        foreach (var roleName in new[] { RoleNames.Admin, RoleNames.Support, RoleNames.User })
        {
            if (!await context.Roles.AnyAsync(r => r.RoleName == roleName))
                context.Roles.Add(new Role { RoleId = Guid.NewGuid(), RoleName = roleName });
        }
        await context.SaveChangesAsync();

        var adminUsername = configuration["Seed:AdminUsername"];
        var adminPassword = configuration["Seed:AdminPassword"];
        if (string.IsNullOrWhiteSpace(adminUsername) || string.IsNullOrWhiteSpace(adminPassword))
            return;

        if (await context.Users.AnyAsync(u => u.Username == adminUsername))
            return;

        var adminRole = await context.Roles.FirstAsync(r => r.RoleName == RoleNames.Admin);
        var admin = new User
        {
            UserId = Guid.NewGuid(),
            Username = adminUsername,
            Email = configuration["Seed:AdminEmail"],
            PasswordHash = hasher.Hash(adminPassword)
        };
        admin.UserRoles.Add(new UserRole { UserId = admin.UserId, RoleId = adminRole.RoleId });

        context.Users.Add(admin);
        await context.SaveChangesAsync();
        logger.LogInformation("Seeded administrator user '{Username}'.", adminUsername);
    }
}
