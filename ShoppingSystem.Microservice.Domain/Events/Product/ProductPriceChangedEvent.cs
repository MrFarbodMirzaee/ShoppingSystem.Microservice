using MediatR;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Events.Product;

/// <summary>
/// Represents a domain event that occurs when a product price is changed.
/// This event contains information about the product, previous price,
/// new price, and notification details.
/// </summary>
public class ProductPriceChangedEvent : INotification
{
    /// <summary>
    /// Initializes a new instance of the ProductPriceChangedEvent class.
    /// </summary>
    /// <param name="productId">The identifier of the product with the price change.</param>
    /// <param name="email">The email information used for notification purposes.</param>
    /// <param name="name">The name of the product.</param>
    /// <param name="oldPrice">The previous product price.</param>
    /// <param name="newPrice">The updated product price.</param>
    public ProductPriceChangedEvent(
        Guid productId,
        Email email,
        ProductName name,
        Money oldPrice,
        Money newPrice)
    {
        ProductId = productId;
        Name = name;
        OldPrice = oldPrice;
        NewPrice = newPrice;
        Email = email;
    }


    /// <summary>
    /// Gets the identifier of the product whose price was changed.
    /// </summary>
    public Guid ProductId { get; }


    /// <summary>
    /// Gets the product name.
    /// </summary>
    public ProductName Name { get; }


    /// <summary>
    /// Gets the previous product price before the change.
    /// </summary>
    public Money OldPrice { get; }


    /// <summary>
    /// Gets the new product price after the change.
    /// </summary>
    public Money NewPrice { get; }


    /// <summary>
    /// Gets the email information associated with the notification.
    /// </summary>
    public Email Email { get; }
}