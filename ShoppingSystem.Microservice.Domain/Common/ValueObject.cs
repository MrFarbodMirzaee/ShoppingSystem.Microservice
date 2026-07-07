namespace ShoppingSystem.Microservice.Domain.Common;

/// <summary>
/// Represents the base class for value objects in the domain model.
/// Value objects are immutable objects that are identified by their values
/// rather than a unique identifier.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Defines the components used to compare value object equality.
    /// Derived classes must provide their value-based equality members.
    /// </summary>
    /// <returns>The collection of components used for equality comparison.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();


    /// <summary>
    /// Determines whether the current value object is equal to another object.
    /// Equality is based on the values returned by GetEqualityComponents.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>True if both objects have the same values; otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }


    /// <summary>
    /// Generates a hash code based on the value object's equality components.
    /// </summary>
    /// <returns>A hash code representing the value object's values.</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
                HashCode.Combine(current, obj));
    }


    /// <summary>
    /// Determines whether two value objects are equal.
    /// </summary>
    /// <param name="left">The first value object.</param>
    /// <param name="right">The second value object.</param>
    /// <returns>True if both value objects are equal; otherwise false.</returns>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }


    /// <summary>
    /// Determines whether two value objects are not equal.
    /// </summary>
    /// <param name="left">The first value object.</param>
    /// <param name="right">The second value object.</param>
    /// <returns>True if the value objects are different; otherwise false.</returns>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }
}