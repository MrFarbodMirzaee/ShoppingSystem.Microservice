using FluentAssertions;
using ShoppingSystem.Microservice.Domain.Aggregates.Product;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Test.Aggregates;

public class ProductTest
{
    [Fact]
    public void Create_Should_CreateProduct_WhenArgumentsAreValid()
    {
        // Arrange
        var name = ProductName.Create("Laptop");
        var description = "Gaming Laptop";
        var price = Money.Create(1500, "USD");
        var categoryId = Guid.NewGuid();

        // Act
        var product = Product.Create(
            name,
            description,
            price,
            categoryId);

        // Assert
        product.Name.Should().Be(name);
        product.Description.Should().Be(description);
        product.Price.Should().Be(price);
        product.CategoryId.Should().Be(categoryId);
        product.IsAvailable.Should().BeTrue();
        product.Images.Should().BeEmpty();
    }

    [Fact]
    public void Update_Should_UpdateProductProperties()
    {
        // Arrange
        var product = Product.Create(
            ProductName.Create("Laptop"),
            "Gaming Laptop",
            Money.Create(1500, "USD"),
            Guid.NewGuid());

        var newName = ProductName.Create("MacBook Pro");
        var newDescription = "Apple Laptop";
        var newPrice = Money.Create(2500, "USD");
        var newCategoryId = Guid.NewGuid();

        // Act
        product.Update(
            newName,
            newDescription,
            newPrice,
            newCategoryId,
            false);

        // Assert
        product.Name.Should().Be(newName);
        product.Description.Should().Be(newDescription);
        product.Price.Should().Be(newPrice);
        product.CategoryId.Should().Be(newCategoryId);
        product.IsAvailable.Should().BeFalse();
    }

    [Fact]
    public void Create_Should_SetProductAvailableByDefault()
    {
        // Arrange & Act
        var product = Product.Create(
            ProductName.Create("Keyboard"),
            "Mechanical Keyboard",
            Money.Create(120, "USD"),
            Guid.NewGuid());

        // Assert
        product.IsAvailable.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_CreateProductWithEmptyImages()
    {
        // Arrange & Act
        var product = Product.Create(
            ProductName.Create("Mouse"),
            "Wireless Mouse",
            Money.Create(50, "USD"),
            Guid.NewGuid());

        // Assert
        product.Images.Should().NotBeNull();
        product.Images.Should().BeEmpty();
    }

    [Fact]
    public void Update_Should_ChangeAvailability()
    {
        // Arrange
        var product = Product.Create(
            ProductName.Create("Monitor"),
            "27 inch Monitor",
            Money.Create(300, "USD"),
            Guid.NewGuid());

        // Act
        product.Update(
            product.Name,
            product.Description,
            product.Price,
            product.CategoryId,
            false);

        // Assert
        product.IsAvailable.Should().BeFalse();
    }
}