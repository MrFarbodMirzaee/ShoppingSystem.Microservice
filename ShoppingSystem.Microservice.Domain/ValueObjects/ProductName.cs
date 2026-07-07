using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.ValueObjects;

/// <summary>
/// Represents a product name as a value object.
/// Encapsulates product name validation and provides value-based equality comparison.
/// </summary>
public class ProductName : ValueObject
{
    /// <summary>
    /// Gets the product name value.
    /// </summary>
    public string Value { get; }


    /// <summary>
    /// Private constructor used to create a validated product name value object.
    /// </summary>
    /// <param name="value">The product name value.</param>
    private ProductName(string value)
    {
        Value = value;
    }


    /// <summary>
    /// Creates a new ProductName value object after validating the provided name.
    /// </summary>
    /// <param name="value">The product name value.</param>
    /// <returns>A validated ProductName value object.</returns>
    public static ProductName Create(string value)
    {
        // Ensures that a product name is provided.
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception("Product name cannot be empty.");

        value = value.Trim();


        // Ensures that the product name meets the minimum length requirement.
        if (value.Length < 2)
            throw new Exception("Product name must be at least 2 characters.");


        // Ensures that the product name does not exceed the maximum allowed length.
        if (value.Length > 100)
            throw new Exception("Product name cannot exceed 100 characters.");

        return new ProductName(value);
    }


    /// <summary>
    /// Defines the components used to compare ProductName value objects.
    /// </summary>
    /// <returns>The equality comparison components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }


    /// <summary>
    /// Returns the string representation of the product name.
    /// </summary>
    /// <returns>The product name value.</returns>
    public override string ToString() => Value;


    /// <summary>
    /// Defines an implicit conversion from ProductName to string.
    /// </summary>
    /// <param name="productName">The product name instance.</param>
    public static implicit operator string(ProductName productName)
        => productName.Value;
}