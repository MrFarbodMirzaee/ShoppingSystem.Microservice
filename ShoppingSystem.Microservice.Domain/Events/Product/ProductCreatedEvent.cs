using MediatR;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Events.Product;

/// <summary>
/// Represents a domain event that occurs when a new product is created.
/// This event can be published through MediatR to notify other parts of the system
/// about the newly created product.
/// </summary>
/// <param name="ProductId">The identifier of the created product.</param>
/// <param name="Name">The name of the created product.</param>
/// <param name="Price">The price information of the created product.</param>
/// <param name="AdminEmails">The list of administrator email addresses to notify.</param>
public record ProductCreatedEvent(
    Guid ProductId,
    ProductName Name,
    Money Price,
    List<string> AdminEmails
) : INotification;