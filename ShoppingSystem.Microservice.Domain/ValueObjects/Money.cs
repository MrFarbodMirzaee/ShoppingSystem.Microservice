using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.ValueObjects;

/// <summary>
/// Represents a monetary value as a value object.
/// Encapsulates an amount and its associated currency while ensuring
/// value-based equality comparison.
/// </summary>
public class Money : ValueObject
{
    /// <summary>
    /// Gets the monetary amount.
    /// </summary>
    public decimal Amount { get; }


    /// <summary>
    /// Gets the currency code associated with the amount.
    /// </summary>
    public string Currency { get; }


    /// <summary>
    /// Private constructor used to create a validated money value object.
    /// </summary>
    /// <param name="amount">The monetary amount.</param>
    /// <param name="currency">The currency code.</param>
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }


    /// <summary>
    /// Creates a new Money value object after validating the amount and currency.
    /// </summary>
    /// <param name="amount">The monetary amount.</param>
    /// <param name="currency">The currency code.</param>
    /// <returns>A validated Money value object.</returns>
    public static Money Create(decimal amount, string currency)
    {
        // Ensures that the monetary amount is not negative.
        if (amount < 0)
            throw new Exception("Amount cannot be negative.");


        // Ensures that a valid currency value is provided.
        if (string.IsNullOrWhiteSpace(currency))
            throw new Exception("Currency is required.");

        return new Money(amount, currency);
    }


    /// <summary>
    /// Defines the components used to compare Money value objects.
    /// </summary>
    /// <returns>The equality comparison components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}