namespace ShoppingSystem.Microservice.Domain.Common;

/// <summary>
/// Represents the base class for domain events.
/// Provides common event metadata such as a unique identifier and occurrence timestamp.
/// </summary>
public class DomainEvent : IDomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the domain event.
    /// Used to distinguish each event instance.
    /// </summary>
    public Guid EventId { get; } = Guid.NewGuid();


    /// <summary>
    /// Gets the date and time when the domain event occurred.
    /// The timestamp is stored using UTC time.
    /// </summary>
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}