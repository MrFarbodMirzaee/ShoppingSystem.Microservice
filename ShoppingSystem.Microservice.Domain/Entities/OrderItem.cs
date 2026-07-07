using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Entities;

/// <summary>
/// Represents an item within an order.
/// Stores product information, pricing details, and quantity required
/// for order processing and total price calculations.
/// </summary>
public class OrderItem : BaseEntity
{
    // Private constructor required by ORM frameworks for entity materialization.
    private OrderItem()
    {
    }


    /// <summary>
    /// Creates a new order item instance.
    /// </summary>
    /// <param name="productId">The identifier of the product.</param>
    /// <param name="productName">The name of the product.</param>
    /// <param name="price">The product price.</param>
    /// <param name="quantity">The product quantity.</param>
    private OrderItem(
        Guid productId,
        string productName,
        Money price,
        int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
    }


    /// <summary>
    /// Creates and validates a new order item.
    /// </summary>
    /// <param name="productId">The identifier of the product.</param>
    /// <param name="productName">The product name.</param>
    /// <param name="price">The product price.</param>
    /// <param name="quantity">The requested quantity.</param>
    /// <returns>A new OrderItem instance.</returns>
    public static OrderItem Create(
        Guid productId,
        string productName,
        Money price,
        int quantity)
    {
        // Ensures that the order item has a valid quantity.
        if (quantity <= 0)
            throw new Exception("Quantity must be positive.");

        return new OrderItem(
            productId,
            productName,
            price,
            quantity);
    }


    /// <summary>
    /// Increases the quantity of this order item.
    /// </summary>
    /// <param name="quantity">The amount to increase.</param>
    public void IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
    }


    /// <summary>
    /// Gets the identifier of the related product.
    /// </summary>
    public Guid ProductId { get; private set; }


    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public string ProductName { get; private set; }


    /// <summary>
    /// Gets the price of the product.
    /// </summary>
    public Money Price { get; private set; }


    /// <summary>
    /// Gets the quantity of the product in the order.
    /// </summary>
    public int Quantity { get; private set; }


    /// <summary>
    /// Gets the identifier of the related order.
    /// </summary>
    public Guid OrderId { get; private set; }

}