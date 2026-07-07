using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.ValueObjects;

/// <summary>
/// Represents a user password as a value object.
/// Encapsulates password validation, hashing, verification, and value-based equality.
/// </summary>
public class Password : ValueObject
{
    /// <summary>
    /// Gets the hashed password value.
    /// </summary>
    private string Hash { get; }


    /// <summary>
    /// Private constructor used to create a password value object from a hash.
    /// </summary>
    /// <param name="hash">The hashed password value.</param>
    private Password(string hash)
    {
        Hash = hash;
    }


    /// <summary>
    /// Creates a new Password value object from a plain text password.
    /// The password is validated and securely hashed before storage.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <returns>A hashed Password value object.</returns>
    public static Password Create(string password)
    {
        // Ensures that a password value is provided.
        if (string.IsNullOrWhiteSpace(password))
            throw new Exception("Password cannot be empty.");


        // Ensures that the password meets the minimum length requirement.
        if (password.Length < 8)
            throw new Exception("Password must be at least 8 characters.");


        // Creates a secure password hash using BCrypt.
        // Replace with your real hashing service (BCrypt, Argon2, PBKDF2, etc.)
        var hash = BCrypt.Net.BCrypt.HashPassword(password);

        return new Password(hash);
    }


    /// <summary>
    /// Creates a Password value object from an existing password hash.
    /// </summary>
    /// <param name="hash">The existing hashed password value.</param>
    /// <returns>A Password value object.</returns>
    public static Password CreateFromHash(string hash)
    {
        // Ensures that a password hash is provided.
        if (string.IsNullOrWhiteSpace(hash))
            throw new Exception("Password hash cannot be empty.");

        return new Password(hash);
    }


    /// <summary>
    /// Verifies whether a provided plain text password matches the stored hash.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <returns>True if the password matches; otherwise, false.</returns>
    public bool Verify(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Hash);
    }


    /// <summary>
    /// Defines the components used to compare Password value objects.
    /// </summary>
    /// <returns>The equality comparison components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Hash;
    }


    /// <summary>
    /// Returns the string representation of the password hash.
    /// </summary>
    /// <returns>The hashed password value.</returns>
    public override string ToString() => Hash;


    /// <summary>
    /// Defines an implicit conversion from Password to string.
    /// </summary>
    /// <param name="password">The password instance.</param>
    public static implicit operator string(Password password)
        => password.Hash;
}