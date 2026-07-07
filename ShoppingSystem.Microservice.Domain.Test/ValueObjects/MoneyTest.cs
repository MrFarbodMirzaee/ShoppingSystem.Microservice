using FluentAssertions;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Test.ValueObjects;

public class MoneyTest
{
    [Fact]
    public void Create_Should_CreateMoney_WhenArgumentsAreValid()
    {
        // Arrange
        const decimal amount = 100.50m;
        const string currency = "USD";

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        money.Amount.Should().Be(amount);
        money.Currency.Should().Be(currency);
    }

    [Fact]
    public void Create_Should_ThrowException_WhenAmountIsNegative()
    {
        // Act
        var action = () => Money.Create(-1m, "USD");

        // Assert
        action.Should()
            .Throw<Exception>()
            .WithMessage("Amount cannot be negative.");
    }

    [Fact]
    public void Create_Should_ThrowException_WhenCurrencyIsEmpty()
    {
        // Act
        var action = () => Money.Create(100m, "");

        // Assert
        action.Should()
            .Throw<Exception>()
            .WithMessage("Currency is required.");
    }

    [Fact]
    public void Create_Should_ThrowException_WhenCurrencyIsWhiteSpace()
    {
        // Act
        var action = () => Money.Create(100m, "   ");

        // Assert
        action.Should()
            .Throw<Exception>()
            .WithMessage("Currency is required.");
    }

    [Fact]
    public void Equality_Should_BeTrue_WhenValuesAreEqual()
    {
        // Arrange
        var first = Money.Create(100m, "USD");
        var second = Money.Create(100m, "USD");

        // Assert
        first.Should().Be(second);
        first.Equals(second).Should().BeTrue();
    }

    [Fact]
    public void Equality_Should_BeFalse_WhenAmountsDiffer()
    {
        // Arrange
        var first = Money.Create(100m, "USD");
        var second = Money.Create(200m, "USD");

        // Assert
        first.Should().NotBe(second);
    }

    [Fact]
    public void Equality_Should_BeFalse_WhenCurrenciesDiffer()
    {
        // Arrange
        var first = Money.Create(100m, "USD");
        var second = Money.Create(100m, "EUR");

        // Assert
        first.Should().NotBe(second);
    }
    
    [Fact]
    public void EqualityOperator_Should_ReturnTrue_WhenValuesAreEqual()
    {
        var first = Money.Create(100m, "USD");
        var second = Money.Create(100m, "USD");

        (first == second).Should().BeTrue();
    }

    [Fact]
    public void InequalityOperator_Should_ReturnTrue_WhenValuesAreDifferent()
    {
        var first = Money.Create(100m, "USD");
        var second = Money.Create(200m, "USD");

        (first != second).Should().BeTrue();
    }
}