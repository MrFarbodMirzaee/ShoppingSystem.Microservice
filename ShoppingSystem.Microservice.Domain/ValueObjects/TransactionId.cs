using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.ValueObjects;

/// <summary>
/// Represents a transaction identifier as a value object.
/// Encapsulates transaction ID generation, validation, and value-based equality comparison.
/// </summary>
public class TransactionId : ValueObject
{
    /// <summary>
    /// Gets the transaction identifier value.
    /// </summary>
    public string Value { get; }


    /// <summary>
    /// Private constructor used to create a transaction identifier value object.
    /// </summary>
    /// <param name="value">The transaction identifier value.</param>
    private TransactionId(string value)
    {
        Value = value;
    }


    /// <summary>
    /// Creates a new transaction identifier with a generated unique value.
    /// </summary>
    /// <returns>A new TransactionId value object.</returns>
    public static TransactionId Create()
    {
        var value = $"TXN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

        return new TransactionId(value);
    }


    /// <summary>
    /// Creates a new transaction identifier from an existing value after validation.
    /// </summary>
    /// <param name="value">The transaction identifier value.</param>
    /// <returns>A validated TransactionId value object.</returns>
    public static TransactionId Create(string value)
    {
        // Ensures that the transaction identifier value is provided.
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception("Transaction ID cannot be empty.");

        return new TransactionId(value.Trim());
    }


    /// <summary>
    /// Defines the components used to compare TransactionId value objects.
    /// </summary>
    /// <returns>The equality comparison components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }


    /// <summary>
    /// Returns the string representation of the transaction identifier.
    /// </summary>
    /// <returns>The transaction identifier value.</returns>
    public override string ToString() => Value;


    /// <summary>
    /// Defines an implicit conversion from TransactionId to string.
    /// </summary>
    /// <param name="transactionId">The transaction identifier instance.</param>
    public static implicit operator string(TransactionId transactionId)
        => transactionId.Value;
}