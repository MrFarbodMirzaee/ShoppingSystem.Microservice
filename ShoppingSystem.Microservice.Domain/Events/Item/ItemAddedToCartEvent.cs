using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Events.Item;

/// <summary>
/// Represents a domain event that occurs when an item is added to a cart.
/// This event contains information about the cart, product, and quantity added.
/// </summary>
public class ItemAddedToCartEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the ItemAddedToCartEvent class.
    /// </summary>
    /// <param name="cartId">The identifier of the cart where the item was added.</param>
    /// <param name="productId">The identifier of the added product.</param>
    /// <param name="quantity">The quantity of the added product.</param>
    public ItemAddedToCartEvent(
        Guid cartId,
        Guid productId,
        Quantity quantity)
    {
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
        AddedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Gets the identifier of the cart where the item was added.
    /// </summary>
    public Guid CartId { get; }


    /// <summary>
    /// Gets the identifier of the added product.
    /// </summary>
    public Guid ProductId { get; }


    /// <summary>
    /// Gets the quantity of the added product.
    /// </summary>
    public Quantity Quantity { get; }


    /// <summary>
    /// Gets the date and time when the item was added to the cart.
    /// </summary>
    public DateTime AddedAt { get; }

}