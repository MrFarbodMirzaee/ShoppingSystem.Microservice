using FluentAssertions;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Test.ValueObjects;

public class QuantityTest
{
    [Fact]
    public void Create_Should_CreateQuantity_WhenValueIsValid()
    {
        // Act
        var quantity = Quantity.Create(5);

        // Assert
        quantity.Value.Should().Be(5);
    }

    [Fact]
    public void Create_Should_ThrowException_WhenValueIsZero()
    {
        // Arrange
        Action act = () => Quantity.Create(0);

        // Assert
        act.Should()
            .Throw<Exception>()
            .WithMessage("Quantity must be greater than zero.");
    }

    [Fact]
    public void Increase_Should_ReturnNewQuantity_WhenAmountIsValid()
    {
        // Arrange
        var quantity = Quantity.Create(5);

        // Act
        var result = quantity.Increase(3);

        // Assert
        result.Value.Should().Be(8);
    }

    [Fact]
    public void Increase_Should_NotModifyOriginalQuantity()
    {
        // Arrange
        var quantity = Quantity.Create(5);

        // Act
        var result = quantity.Increase(3);

        // Assert
        quantity.Value.Should().Be(5);
        result.Value.Should().Be(8);
    }

    [Fact]
    public void Increase_Should_ThrowException_WhenAmountIsZero()
    {
        // Arrange
        var quantity = Quantity.Create(5);

        // Act
        Action act = () => quantity.Increase(0);

        // Assert
        act.Should()
            .Throw<Exception>()
            .WithMessage("Increase amount must be greater than zero.");
    }

    [Fact]
    public void Decrease_Should_ReturnNewQuantity_WhenAmountIsValid()
    {
        // Arrange
        var quantity = Quantity.Create(10);

        // Act
        var result = quantity.Decrease(3);

        // Assert
        result.Value.Should().Be(7);
    }

    [Fact]
    public void Decrease_Should_ThrowException_WhenAmountIsZero()
    {
        // Arrange
        var quantity = Quantity.Create(10);

        // Act
        Action act = () => quantity.Decrease(0);

        // Assert
        act.Should()
            .Throw<Exception>()
            .WithMessage("Decrease amount must be greater than zero.");
    }

    [Fact]
    public void Decrease_Should_ThrowException_WhenAmountIsGreaterThanOrEqualToValue()
    {
        // Arrange
        var quantity = Quantity.Create(10);

        // Act
        Action act = () => quantity.Decrease(10);

        // Assert
        act.Should()
            .Throw<Exception>()
            .WithMessage("Quantity cannot be zero or negative.");
    }

    [Fact]
    public void Equality_Should_ReturnTrue_WhenValuesAreSame()
    {
        // Arrange
        var first = Quantity.Create(5);
        var second = Quantity.Create(5);

        // Assert
        first.Should().Be(second);
    }

    [Fact]
    public void Equality_Should_ReturnFalse_WhenValuesAreDifferent()
    {
        // Arrange
        var first = Quantity.Create(5);
        var second = Quantity.Create(10);

        // Assert
        first.Should().NotBe(second);
    }

    [Fact]
    public void ToString_Should_ReturnValueAsString()
    {
        // Arrange
        var quantity = Quantity.Create(5);

        // Act
        var result = quantity.ToString();

        // Assert
        result.Should().Be("5");
    }

    [Fact]
    public void ImplicitConversion_Should_ReturnByteValue()
    {
        // Arrange
        var quantity = Quantity.Create(5);

        // Act
        byte result = quantity;

        // Assert
        result.Should().Be(5);
    }
}