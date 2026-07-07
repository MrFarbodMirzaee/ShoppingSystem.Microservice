namespace ShoppingSystem.Microservice.Domain.Common;

/// <summary>
/// Represents the base class for all domain entities.
/// Provides common properties and behaviors shared across entities,
/// including identity, creation information, and soft deletion support.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the BaseEntity class.
    /// Sets the entity as active by default.
    /// </summary>
    protected BaseEntity()
    {
        IsDeleted = false;
    }


    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public Guid Id { get; }


    /// <summary>
    /// Gets the date and time when the entity was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }
    

    /// <summary>
    /// Gets whether the entity has been soft deleted.
    /// </summary>
    public bool IsDeleted { get; private set; }
    

    /// <summary>
    /// Marks the entity as deleted without physically removing it from storage.
    /// </summary>
    public void Delete()
    {
        IsDeleted = true;
    }
}