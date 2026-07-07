using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Events.Item;

/// <summary>
/// Represents a domain event that occurs when an item is removed from a cart.
/// Contains information about the affected cart, product, and removed quantity.
/// </summary>
public class ItemRemovedFromCartEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the ItemRemovedFromCartEvent class.
    /// </summary>
    /// <param name="cartId">The identifier of the cart where the item was removed.</param>
    /// <param name="productId">The identifier of the removed product.</param>
    /// <param name="quantity">The quantity of the removed product.</param>
    public ItemRemovedFromCartEvent(
        Guid cartId,
        Guid productId,
        Quantity quantity)
    {
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
        RemovedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Gets the identifier of the cart where the item was removed.
    /// </summary>
    public Guid CartId { get; }


    /// <summary>
    /// Gets the identifier of the removed product.
    /// </summary>
    public Guid ProductId { get; }


    /// <summary>
    /// Gets the quantity of the removed product.
    /// </summary>
    public Quantity Quantity { get; }


    /// <summary>
    /// Gets the date and time when the item was removed from the cart.
    /// </summary>
    public DateTime RemovedAt { get; }
}