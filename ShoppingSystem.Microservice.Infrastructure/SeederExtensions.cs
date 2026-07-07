using Infrastructure.Contexts;
using Infrastructure.Persistence.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;

namespace Infrastructure;

public static class SeederExtensions
{
    public static async Task UseDatabaseSeederAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();
        
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();
        
        await context.Database.MigrateAsync();

        await ShoppingSystemSeeder.SeedAsync(context, userManager);
    }
}