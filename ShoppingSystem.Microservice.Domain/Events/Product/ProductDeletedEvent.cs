using MediatR;

namespace ShoppingSystem.Microservice.Domain.Events.Product;

/// <summary>
/// Represents a domain event that occurs when a product is deleted.
/// This event can be published through MediatR to notify other parts
/// of the system about the product deletion.
/// </summary>
/// <param name="ProductId">The identifier of the deleted product.</param>
public record ProductDeletedEvent(
    Guid ProductId
): INotification;