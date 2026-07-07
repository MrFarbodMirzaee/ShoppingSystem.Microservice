using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.ValueObjects;

/// <summary>
/// Represents a quantity value as a value object.
/// Encapsulates quantity validation and provides operations for increasing
/// and decreasing the quantity while maintaining valid state.
/// </summary>
public class Quantity : ValueObject
{
    /// <summary>
    /// Gets the quantity value.
    /// </summary>
    public byte Value { get; }


    /// <summary>
    /// Private constructor used to create a validated quantity value object.
    /// </summary>
    /// <param name="value">The quantity value.</param>
    private Quantity(byte value)
    {
        Value = value;
    }
    

    /// <summary>
    /// Increases the current quantity by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to increase.</param>
    /// <returns>A new Quantity value object with the increased value.</returns>
    public Quantity Increase(byte amount)
    {
        // Ensures that the increase amount is greater than zero.
        if (amount == 0)
            throw new Exception("Increase amount must be greater than zero.");

        return new Quantity((byte)(Value + amount));
    }


    /// <summary>
    /// Decreases the current quantity by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to decrease.</param>
    /// <returns>A new Quantity value object with the decreased value.</returns>
    public Quantity Decrease(byte amount)
    {
        // Ensures that the decrease amount is greater than zero.
        if (amount == 0)
            throw new Exception("Decrease amount must be greater than zero.");


        // Prevents the quantity from becoming zero or negative.
        if (amount >= Value)
            throw new Exception("Quantity cannot be zero or negative.");

        return new Quantity((byte)(Value - amount));
    }


    /// <summary>
    /// Creates a new Quantity value object after validating the provided value.
    /// </summary>
    /// <param name="value">The quantity value.</param>
    /// <returns>A validated Quantity value object.</returns>
    public static Quantity Create(byte value)
    {
        // Ensures that the quantity is greater than zero.
        if (value == 0)
            throw new Exception("Quantity must be greater than zero.");

        return new Quantity(value);
    }


    /// <summary>
    /// Defines the components used to compare Quantity value objects.
    /// </summary>
    /// <returns>The equality comparison components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }


    /// <summary>
    /// Returns the string representation of the quantity value.
    /// </summary>
    /// <returns>The quantity value as a string.</returns>
    public override string ToString() => Value.ToString();


    /// <summary>
    /// Defines an implicit conversion from Quantity to byte.
    /// </summary>
    /// <param name="quantity">The quantity instance.</param>
    public static implicit operator byte(Quantity quantity)
        => quantity.Value;
}