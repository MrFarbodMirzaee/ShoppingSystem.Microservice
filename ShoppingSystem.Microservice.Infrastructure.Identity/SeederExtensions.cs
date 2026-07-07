using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingSystem.Microservice.Infrastructure.Identity.Context;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Seeds;

namespace ShoppingSystem.Microservice.Infrastructure.Identity;

public static class SeederExtensions
{
    public static async Task UseIdentitySeederAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<IdentityAppDbContext>();
        
        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        await context.Database.MigrateAsync();

        await IdentitySeed
            .SeedAsync(roleManager, userManager);
    }
}