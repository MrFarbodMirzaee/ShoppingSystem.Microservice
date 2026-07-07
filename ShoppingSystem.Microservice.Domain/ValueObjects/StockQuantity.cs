using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.ValueObjects;

/// <summary>
/// Represents a stock quantity value as a value object.
/// Encapsulates stock quantity operations and provides value-based equality comparison.
/// </summary>
public class StockQuantity : ValueObject
{
    /// <summary>
    /// Gets the stock quantity value.
    /// </summary>
    public byte Value { get; }


    /// <summary>
    /// Private constructor used to create a stock quantity value object.
    /// </summary>
    /// <param name="value">The stock quantity value.</param>
    private StockQuantity(byte value)
    {
        Value = value;
    }


    /// <summary>
    /// Creates a new StockQuantity value object.
    /// </summary>
    /// <param name="value">The stock quantity value.</param>
    /// <returns>A new StockQuantity value object.</returns>
    public static StockQuantity Create(byte value)
    {
        return new StockQuantity(value);
    }


    /// <summary>
    /// Increases the current stock quantity by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to increase.</param>
    /// <returns>A new StockQuantity value object with the increased value.</returns>
    public StockQuantity Increase(byte amount)
    {
        return new StockQuantity((byte)(Value + amount));
    }


    /// <summary>
    /// Decreases the current stock quantity by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to decrease.</param>
    /// <returns>A new StockQuantity value object with the decreased value.</returns>
    public StockQuantity Decrease(byte amount)
    {
        // Prevents the stock quantity from becoming negative.
        if (amount > Value)
            throw new Exception("Stock quantity cannot be negative.");

        return new StockQuantity((byte)(Value - amount));
    }


    /// <summary>
    /// Defines the components used to compare StockQuantity value objects.
    /// </summary>
    /// <returns>The equality comparison components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }


    /// <summary>
    /// Returns the string representation of the stock quantity value.
    /// </summary>
    /// <returns>The stock quantity value as a string.</returns>
    public override string ToString() => Value.ToString();


    /// <summary>
    /// Defines an implicit conversion from StockQuantity to byte.
    /// </summary>
    /// <param name="stockQuantity">The stock quantity instance.</param>
    public static implicit operator byte(StockQuantity stockQuantity)
        => stockQuantity.Value;
}