using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.ValueObjects;

/// <summary>
/// Represents an email address as a value object.
/// Encapsulates email validation and ensures email equality is based on its value.
/// </summary>
public class Email: ValueObject
{
    /// <summary>
    /// Gets the normalized email address value.
    /// </summary>
    public string Value { get; }


    /// <summary>
    /// Private constructor used to create a validated email value object.
    /// </summary>
    /// <param name="value">The email address value.</param>
    private Email(string value)
    {
        Value = value;
    }


    /// <summary>
    /// Creates a new Email value object after validating the provided email address.
    /// </summary>
    /// <param name="value">The email address to validate.</param>
    /// <returns>A validated Email value object.</returns>
    public static Email Create(string value)
    {
        // Ensures that an email address is provided.
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception("Email is required.");


        // Ensures that the provided value has a basic email format.
        if (!value.Contains('@'))
            throw new Exception("Invalid email.");

        return new Email(value.Trim().ToLower());
    }


    /// <summary>
    /// Defines the components used to compare email value objects.
    /// </summary>
    /// <returns>The equality comparison components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}