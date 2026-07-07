using System.Text;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Domain.Aggregates.Cart;
using ShoppingSystem.Microservice.Domain.Aggregates.Inventory;
using ShoppingSystem.Microservice.Domain.Aggregates.Order;
using ShoppingSystem.Microservice.Domain.Aggregates.Payment;
using ShoppingSystem.Microservice.Domain.Aggregates.Product;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;

namespace Infrastructure.Persistence.Seeds;

public static class ShoppingSystemSeeder
{
    public static async Task SeedAsync(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        // 🔥 INTERNAL LOGGER (NO SIGNATURE CHANGE)
        var loggerFactory = new ServiceCollection()
            .BuildServiceProvider()
            .GetService<ILoggerFactory>();

        var logger = loggerFactory?.CreateLogger("ShoppingSystemSeeder");

        logger?.LogInformation("ShoppingSystem seeding started");

        // =========================
        // 0. IDENTITY USERS
        // =========================
        var adminUser = await userManager.FindByEmailAsync("admin@shopping.com");
        var customerUser = await userManager.FindByEmailAsync("customer@shopping.com");

        if (adminUser is null || customerUser is null)
        {
            logger?.LogError("Identity users not found. Run IdentitySeed first.");
            throw new Exception("Identity users not found. Run IdentitySeed first.");
        }

        // =========================
        // CHECK DB
        // =========================
        if (await context.Product.AnyAsync())
        {
            logger?.LogInformation("Seeding skipped - products already exist");
            return;
        }

        // =========================
        // 1. CATEGORIES
        // =========================
        logger?.LogInformation("Seeding categories");

        var electronics = new Category("Electronics", "Electronic items");
        var clothing = new Category("Clothing", "Fashion items");

        await context.Category.AddRangeAsync(electronics, clothing);

        // =========================
        // 2. ADDRESS
        // =========================
        logger?.LogInformation("Seeding addresses");

        var address1 = new Address(
            "221B Baker Street",
            "London",
            "Greater London",
            "United Kingdom",
            "NW1 6XE");

        var address2 = new Address(
            "1600 Amphitheatre Parkway",
            "Mountain View",
            "California",
            "United States",
            "94043");

        var address3 = new Address(
            "1 Apple Park Way",
            "Cupertino",
            "California",
            "United States",
            "95014");

        var address4 = new Address(
            "10 Downing Street",
            "London",
            "Greater London",
            "United Kingdom",
            "SW1A 2AA");

        var address5 = new Address(
            "350 Fifth Avenue",
            "New York",
            "New York",
            "United States",
            "10118");

        context.Address.AddRange(
            address1,
            address2,
            address3,
            address4,
            address5);

        // =========================
        // 3. PRODUCTS
        // =========================
        logger?.LogInformation("Seeding products");

        var products = new List<Product>
        {
            Product.Create(ProductName.Create("Phone"), "Smart phone", Money.Create(700, "USD"), electronics.Id),
            Product.Create(ProductName.Create("Laptop"), "Gaming laptop", Money.Create(1500, "USD"), electronics.Id),
            Product.Create(ProductName.Create("Tablet"), "Android tablet", Money.Create(400, "USD"), electronics.Id),

            Product.Create(ProductName.Create("T-Shirt"), "Cotton shirt", Money.Create(20, "USD"), clothing.Id),
            Product.Create(ProductName.Create("Shoes"), "Running shoes", Money.Create(90, "USD"), clothing.Id)
        };

        await context.Product.AddRangeAsync(products);

        // =========================
        // 4. INVENTORY
        // =========================
        logger?.LogInformation("Seeding inventory");

        var inventories = products.Select(p =>
            Inventory.Create(
                p.Id,
                StockQuantity.Create(100)
            )
        );

        await context.Inventory.AddRangeAsync(inventories);

        // =========================
        // 5. CART
        // =========================
        logger?.LogInformation("Seeding cart");

        var cart = Cart.Create(customerUser.Id);

        foreach (var p in products.Take(3))
        {
            cart.AddItem(
                p.Id,
                p.Name,
                Quantity.Create(2),
                p.Price.Amount
            );
        }

        await context.Cart.AddAsync(cart);

        // =========================
        // 6. ORDER
        // =========================
        logger?.LogInformation("Seeding order");

        var order = Order.Create(
            customerUser.Id,
            OrderNumber.Create("ORD-1001")
        );

        foreach (var p in products.Take(3))
        {
            order.AddItem(
                p.Id,
                p.Name,
                p.Price,
                Quantity.Create(1)
            );
        }

        await context.Order.AddAsync(order);

        // =========================
        // 7. PAYMENT
        // =========================
        logger?.LogInformation("Seeding payment");

        var payment = Payment.Create(
            order.Id,
            Money.Create(1000, "USD")
        );

        await context.Payment.AddAsync(payment);

        // =========================
        // 8. PRODUCT IMAGES
        // =========================
        logger?.LogInformation("Seeding product images");

        var productImages = new List<ProductImage>();

        var imageData = Encoding.UTF8.GetBytes("fake-image-data");

        foreach (var product in products)
        {
            productImages.Add(new ProductImage(
                product.Id,
                $"{product.Name.Value.ToLower()}-1.jpg",
                $"/images/products/{product.Name.Value.ToLower()}-1.jpg",
                "image/jpeg",
                imageData,
                imageData.Length,
                isMain: true
            ));

            productImages.Add(new ProductImage(
                product.Id,
                $"{product.Name.Value.ToLower()}-2.jpg",
                $"/images/products/{product.Name.Value.ToLower()}-2.jpg",
                "image/jpeg",
                imageData,
                imageData.Length,
                isMain: false
            ));
        }

        await context.Set<ProductImage>().AddRangeAsync(productImages);

        await context.SaveChangesAsync();

        logger?.LogInformation("ShoppingSystem seeding completed");
    }
}