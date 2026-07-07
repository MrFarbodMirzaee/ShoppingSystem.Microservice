using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;

namespace ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Seeds;

public static class IdentitySeed
{
    public static async Task SeedAsync(
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        var loggerFactory = new ServiceCollection()
            .BuildServiceProvider()
            .GetService<ILoggerFactory>();

        var logger = loggerFactory?.CreateLogger("IdentitySeed");

        logger?.LogInformation("Identity seeding started");

        var roles = new[] { "Admin", "Customer", "Seller" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                logger?.LogInformation("Role created: {Role}", role);
            }
        }

        // ADMIN
        const string adminEmail = "admin@shopping.com";
        const string adminPassword = "Admin@123";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator"
            };

            var result = await userManager.CreateAsync(admin, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
                logger?.LogInformation("Admin created: {Email}", adminEmail);
            }
        }

        // CUSTOMER
        const string customerEmail = "customer@shopping.com";
        const string customerPassword = "Customer@123";

        var customer = await userManager.FindByEmailAsync(customerEmail);

        if (customer is null)
        {
            customer = new ApplicationUser
            {
                UserName = customerEmail,
                Email = customerEmail,
                EmailConfirmed = true,
                FirstName = "Default",
                LastName = "Customer"
            };

            var result = await userManager.CreateAsync(customer, customerPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(customer, "Customer");
                logger?.LogInformation("Customer created: {Email}", customerEmail);
            }
        }

        logger?.LogInformation("Identity seeding finished");
    }
}