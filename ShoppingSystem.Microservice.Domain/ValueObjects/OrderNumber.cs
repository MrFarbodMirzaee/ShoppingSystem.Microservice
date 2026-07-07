using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.ValueObjects;

/// <summary>
/// Represents a unique order identifier as a value object.
/// Encapsulates order number generation, validation, and value-based equality.
/// </summary>
public class OrderNumber : ValueObject
{
    /// <summary>
    /// Gets the order number value.
    /// </summary>
    public string Value { get; }


    /// <summary>
    /// Private constructor used to create an order number value object.
    /// </summary>
    /// <param name="value">The order number value.</param>
    private OrderNumber(string value)
    {
        Value = value;
    }


    /// <summary>
    /// Creates a new order number with a generated unique value.
    /// </summary>
    /// <returns>A new OrderNumber value object.</returns>
    public static OrderNumber Create()
    {
        var value = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

        return new OrderNumber(value);
    }


    /// <summary>
    /// Creates a new order number from an existing value after validation.
    /// </summary>
    /// <param name="value">The order number value.</param>
    /// <returns>A validated OrderNumber value object.</returns>
    public static OrderNumber Create(string value)
    {
        // Ensures that the order number value is provided.
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception("Order number cannot be empty.");

        return new OrderNumber(value.Trim().ToUpper());
    }


    /// <summary>
    /// Defines the components used to compare OrderNumber value objects.
    /// </summary>
    /// <returns>The equality comparison components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }


    /// <summary>
    /// Returns the string representation of the order number.
    /// </summary>
    /// <returns>The order number value.</returns>
    public override string ToString() => Value;


    /// <summary>
    /// Defines an implicit conversion from OrderNumber to string.
    /// </summary>
    /// <param name="orderNumber">The order number instance.</param>
    public static implicit operator string(OrderNumber orderNumber)
        => orderNumber.Value;
}