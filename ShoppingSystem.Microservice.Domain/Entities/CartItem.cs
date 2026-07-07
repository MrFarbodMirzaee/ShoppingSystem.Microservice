using ShoppingSystem.Microservice.Domain.Aggregates.Product;
using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Entities;

/// <summary>
/// Represents an item inside a shopping cart.
/// Stores product information, quantity, and pricing details required
/// for cart calculations.
/// </summary>
public class CartItem : BaseEntity
{
    /// <summary>
    /// Gets the identifier of the related product.
    /// </summary>
    public Guid ProductId { get; private set; }


    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public ProductName ProductName { get; private set; }


    /// <summary>
    /// Gets the quantity of the product in the cart.
    /// </summary>
    public Quantity Quantity { get; private set; }


    /// <summary>
    /// Gets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; private set; }


    /// <summary>
    /// Gets the identifier of the related cart.
    /// </summary>
    public Guid CartId { get; private set; }


    /// <summary>
    /// Navigation property representing the related product.
    /// </summary>
    public Product Product { get; private set; }
    

    // Private constructor required by ORM frameworks for entity materialization.
    private CartItem()
    {
        
    }


    /// <summary>
    /// Creates a new cart item instance.
    /// </summary>
    /// <param name="productId">The identifier of the product.</param>
    /// <param name="productName">The product name.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="unitPrice">The product unit price.</param>
    public CartItem(
        Guid productId,
        ProductName productName,
        Quantity quantity,
        decimal unitPrice)
    {
        // Ensures the product has a valid price.
        if (unitPrice <= 0)
            throw new Exception("Product price must be greater than zero.");

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }


    /// <summary>
    /// Creates and validates a new cart item.
    /// </summary>
    /// <param name="productId">The identifier of the product.</param>
    /// <param name="productName">The product name.</param>
    /// <param name="quantity">The product quantity.</param>
    /// <param name="unitPrice">The product unit price.</param>
    /// <returns>A new CartItem instance.</returns>
    public static CartItem Create(
        Guid productId,
        ProductName productName,
        Quantity quantity,
        decimal unitPrice)
    {
        // Ensures the cart item is associated with a valid product.
        if (productId == Guid.Empty)
            throw new Exception("ProductId cannot be empty.");

        // Ensures the unit price is valid.
        if (unitPrice <= 0)
            throw new Exception("Unit price must be greater than zero.");

        return new CartItem(productId, productName, quantity, unitPrice);
    }


    /// <summary>
    /// Increases the quantity of this cart item.
    /// </summary>
    /// <param name="amount">The amount to increase.</param>
    public void IncreaseQuantity(byte amount)
    {
        Quantity = Quantity.Increase(amount);
    }


    /// <summary>
    /// Decreases the quantity of this cart item.
    /// </summary>
    /// <param name="amount">The amount to decrease.</param>
    public void DecreaseQuantity(byte amount)
    {
        Quantity = Quantity.Decrease(amount);
    }
    

    /// <summary>
    /// Updates the quantity of this cart item.
    /// </summary>
    /// <param name="quantity">The new quantity value.</param>
    public void UpdateQuantity(byte quantity)
    {
        Quantity = Quantity.Create(quantity);
    }


    /// <summary>
    /// Calculates the total price of this cart item.
    /// </summary>
    /// <returns>The total price based on unit price and quantity.</returns>
    public decimal GetTotalPrice()
    {
        return UnitPrice * Quantity.Value;
    }
}