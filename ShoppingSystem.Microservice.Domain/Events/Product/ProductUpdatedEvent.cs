using MediatR;

namespace ShoppingSystem.Microservice.Domain.Events.Product;

/// <summary>
/// Represents a domain event that occurs when a product is updated.
/// This event contains the updated product information and can be published
/// through MediatR to notify other parts of the system about the change.
/// </summary>
/// <param name="ProductId">The identifier of the updated product.</param>
/// <param name="Name">The updated product name.</param>
/// <param name="Amount">The updated product price amount.</param>
/// <param name="Currency">The currency of the updated product price.</param>
public record ProductUpdatedEvent(
    Guid ProductId,
    string Name,
    decimal Amount,
    string Currency
) : INotification;