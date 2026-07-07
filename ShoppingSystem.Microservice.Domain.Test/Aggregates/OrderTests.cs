using FluentAssertions;
using ShoppingSystem.Microservice.Domain.Aggregates.Order;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Test.Aggregates;

public class OrderTests
{
    [Fact]
    public void Create_Should_CreateOrder_WhenArgumentsAreValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var orderNumber = OrderNumber.Create();

        // Act
        var order = Order.Create(userId, orderNumber);

        // Assert
        order.UserId.Should().Be(userId);
        order.OrderNumber.Should().Be(orderNumber);
        order.Status.Should().Be(OrderStatus.Pending);
        order.TotalPrice.Amount.Should().Be(0);
        order.TotalPrice.Currency.Should().Be("USD");
        order.Items.Should().BeEmpty();
    }

    [Fact]
    public void UpdateStatus_Should_UpdateOrderStatus()
    {
        // Arrange
        var order = Order.Create(
            Guid.NewGuid(),
            OrderNumber.Create());

        // Act
        order.UpdateStatus(OrderStatus.Paid);

        // Assert
        order.Status.Should().Be(OrderStatus.Paid);
    }

    [Fact]
    public void AddItem_Should_AddNewItem()
    {
        // Arrange
        var order = Order.Create(
            Guid.NewGuid(),
            OrderNumber.Create());

        var productId = Guid.NewGuid();

        // Act
        order.AddItem(
            productId,
            ProductName.Create("Laptop"),
            Money.Create(1000, "USD"),
            Quantity.Create(2));

        // Assert
        order.Items.Should().HaveCount(1);

        var item = order.Items.Single();

        item.ProductId.Should().Be(productId);
        item.ProductName.Should().Be(ProductName.Create("Laptop"));
        item.Price.Should().Be(Money.Create(1000, "USD"));
        item.Quantity.Should().Be(Quantity.Create(2));
    }

    [Fact]
    public void AddItem_Should_UpdateTotalPrice()
    {
        // Arrange
        var order = Order.Create(
            Guid.NewGuid(),
            OrderNumber.Create());

        // Act
        order.AddItem(
            Guid.NewGuid(),
            ProductName.Create("Laptop"),
            Money.Create(1000, "USD"),
            Quantity.Create(2));

        // Assert
        order.TotalPrice.Amount.Should().Be(2000);
        order.TotalPrice.Currency.Should().Be("USD");
    }

    [Fact]
    public void AddMultipleItems_Should_CalculateCorrectTotalPrice()
    {
        // Arrange
        var order = Order.Create(
            Guid.NewGuid(),
            OrderNumber.Create());

        // Act
        order.AddItem(
            Guid.NewGuid(),
            ProductName.Create("Laptop"),
            Money.Create(1000, "USD"),
            Quantity.Create(2));

        order.AddItem(
            Guid.NewGuid(),
            ProductName.Create("Mouse"),
            Money.Create(100, "USD"),
            Quantity.Create(3));

        // Assert
        order.Items.Should().HaveCount(2);
        order.TotalPrice.Amount.Should().Be(2300);
    }
}