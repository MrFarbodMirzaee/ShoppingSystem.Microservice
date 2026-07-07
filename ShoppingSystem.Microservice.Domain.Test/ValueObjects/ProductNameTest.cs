using FluentAssertions;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Test.ValueObjects;

public class ProductNameTest
{
    [Fact]
    public void Create_Should_CreateProductName_WhenValueIsValid()
    {
        // Act
        var productName = ProductName.Create("Laptop");

        // Assert
        productName.Value.Should().Be("Laptop");
    }

    [Fact]
    public void Create_Should_TrimValue_WhenValueContainsWhitespace()
    {
        // Act
        var productName = ProductName.Create("  Laptop  ");

        // Assert
        productName.Value.Should().Be("Laptop");
    }

    [Fact]
    public void Create_Should_ThrowException_WhenValueIsEmpty()
    {
        // Arrange
        Action act = () => ProductName.Create("");

        // Assert
        act.Should()
            .Throw<Exception>()
            .WithMessage("Product name cannot be empty.");
    }

    [Fact]
    public void Create_Should_ThrowException_WhenValueIsWhitespace()
    {
        // Arrange
        Action act = () => ProductName.Create("   ");

        // Assert
        act.Should()
            .Throw<Exception>()
            .WithMessage("Product name cannot be empty.");
    }

    [Fact]
    public void Create_Should_ThrowException_WhenValueIsLessThanTwoCharacters()
    {
        // Arrange
        Action act = () => ProductName.Create("A");

        // Assert
        act.Should()
            .Throw<Exception>()
            .WithMessage("Product name must be at least 2 characters.");
    }

    [Fact]
    public void Create_Should_ThrowException_WhenValueExceeds100Characters()
    {
        // Arrange
        var longName = new string('A', 101);

        Action act = () => ProductName.Create(longName);

        // Assert
        act.Should()
            .Throw<Exception>()
            .WithMessage("Product name cannot exceed 100 characters.");
    }

    [Fact]
    public void Equality_Should_ReturnTrue_WhenNamesAreSame()
    {
        // Arrange
        var first = ProductName.Create("Laptop");
        var second = ProductName.Create("Laptop");

        // Assert
        first.Should().Be(second);
    }

    [Fact]
    public void Equality_Should_ReturnFalse_WhenNamesAreDifferent()
    {
        // Arrange
        var first = ProductName.Create("Laptop");
        var second = ProductName.Create("Phone");

        // Assert
        first.Should().NotBe(second);
    }

    [Fact]
    public void ToString_Should_ReturnProductNameValue()
    {
        // Arrange
        var productName = ProductName.Create("Laptop");

        // Act
        var result = productName.ToString();

        // Assert
        result.Should().Be("Laptop");
    }

    [Fact]
    public void ImplicitConversion_Should_ReturnStringValue()
    {
        // Arrange
        var productName = ProductName.Create("Laptop");

        // Act
        string result = productName;

        // Assert
        result.Should().Be("Laptop");
    }
}