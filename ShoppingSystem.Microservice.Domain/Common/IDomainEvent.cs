namespace ShoppingSystem.Microservice.Domain.Common;

/// <summary>
/// Defines the contract for domain events.
/// Domain events represent important business occurrences within the domain
/// and contain common metadata required for event handling.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the unique identifier of the domain event.
    /// </summary>
    Guid EventId { get; }


    /// <summary>
    /// Gets the date and time when the domain event occurred.
    /// </summary>
    DateTimeOffset OccurredOn { get; }
}