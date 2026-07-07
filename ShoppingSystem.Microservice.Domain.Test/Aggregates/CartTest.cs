using FluentAssertions;
using ShoppingSystem.Microservice.Domain.Aggregates.Cart;
using ShoppingSystem.Microservice.Domain.ValueObjects;
using ShoppingSystem.Microservice.Domain.Events.Item;

namespace ShoppingSystem.Microservice.Test.Aggregates;

public class CartTest
{
    [Fact]
    public void Create_Should_CreateCart_WhenCustomerIdIsValid()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var cart = Cart.Create(customerId);

        // Assert
        cart.CustomerId.Should().Be(customerId);
        cart.Items.Should().BeEmpty();
    }

    [Fact]
    public void Create_Should_ThrowException_WhenCustomerIdIsEmpty()
    {
        // Act
        var action = () => Cart.Create(Guid.Empty);

        // Assert
        action.Should()
            .Throw<Exception>()
            .WithMessage("CustomerId cannot be empty.");
    }

    [Fact]
    public void AddItem_Should_AddNewItem_WhenProductDoesNotExist()
    {
        // Arrange
        var cart = Cart.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();

        // Act
        cart.AddItem(
            productId,
            ProductName.Create("Laptop"),
            Quantity.Create(2),
            1000);

        // Assert
        cart.Items.Should().HaveCount(1);

        var item = cart.Items.Single();
        item.ProductId.Should().Be(productId);
        item.ProductName.Should().Be(ProductName.Create("Laptop"));
        item.Quantity.Should().Be(Quantity.Create(2));
    }

    [Fact]
    public void AddItem_Should_IncreaseQuantity_WhenProductAlreadyExists()
    {
        // Arrange
        var cart = Cart.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();

        cart.AddItem(
            productId,
            ProductName.Create("Laptop"),
            Quantity.Create(2),
            1000);

        // Act
        cart.AddItem(
            productId,
            ProductName.Create("Laptop"),
            Quantity.Create(3),
            1000);

        // Assert
        cart.Items.Should().HaveCount(1);
        cart.Items.Single().Quantity.Should().Be(Quantity.Create(5));
    }

    [Fact]
    public void AddItem_Should_AddDomainEvent()
    {
        // Arrange
        var cart = Cart.Create(Guid.NewGuid());

        // Act
        cart.AddItem(
            Guid.NewGuid(),
            ProductName.Create("Laptop"),
            Quantity.Create(1),
            1000);

        // Assert
        cart.DomainEvents.Should()
            .ContainSingle(x => x is ItemAddedToCartEvent);
    }

    [Fact]
    public void RemoveItem_Should_RemoveExistingItem()
    {
        // Arrange
        var cart = Cart.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();

        cart.AddItem(
            productId,
            ProductName.Create("Laptop"),
            Quantity.Create(2),
            1000);

        // Act
        cart.RemoveItem(productId);

        // Assert
        cart.Items.Should().BeEmpty();
    }

    [Fact]
    public void RemoveItem_Should_ThrowException_WhenItemDoesNotExist()
    {
        // Arrange
        var cart = Cart.Create(Guid.NewGuid());

        // Act
        var action = () => cart.RemoveItem(Guid.NewGuid());

        // Assert
        action.Should()
            .Throw<Exception>()
            .WithMessage("Product does not exist in cart.");
    }

    [Fact]
    public void RemoveItem_Should_AddDomainEvent()
    {
        // Arrange
        var cart = Cart.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();

        cart.AddItem(
            productId,
            ProductName.Create("Laptop"),
            Quantity.Create(1),
            1000);

        // Act
        cart.RemoveItem(productId);

        // Assert
        cart.DomainEvents.Should()
            .Contain(x => x is ItemRemovedFromCartEvent);
    }

    [Fact]
    public void GetTotalPrice_Should_ReturnSumOfItems()
    {
        // Arrange
        var cart = Cart.Create(Guid.NewGuid());

        cart.AddItem(
            Guid.NewGuid(),
            ProductName.Create("Laptop"),
            Quantity.Create(2),
            1000);

        cart.AddItem(
            Guid.NewGuid(),
            ProductName.Create("Mouse"),
            Quantity.Create(3),
            100);

        // Act
        var total = cart.GetTotalPrice();

        // Assert
        total.Should().Be(2300);
    }

    [Fact]
    public void Clear_Should_RemoveAllItems()
    {
        // Arrange
        var cart = Cart.Create(Guid.NewGuid());

        cart.AddItem(
            Guid.NewGuid(),
            ProductName.Create("Laptop"),
            Quantity.Create(2),
            1000);

        cart.AddItem(
            Guid.NewGuid(),
            ProductName.Create("Mouse"),
            Quantity.Create(1),
            100);

        // Act
        cart.Clear();

        // Assert
        cart.Items.Should().BeEmpty();
    }
}