using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Aggregates.Inventory;

/// <summary>
/// Represents the Inventory aggregate root.
/// Responsible for managing product stock quantities and stock status changes.
/// </summary>
public class Inventory : AggregateRoot
{
    /// <summary>
    /// Navigation property representing the related product.
    /// </summary>
    public Product.Product Product { get; set; }

    /// <summary>
    /// Gets the identifier of the product associated with this inventory record.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets the current available stock quantity.
    /// </summary>
    public StockQuantity Quantity { get; private set; }

    /// <summary>
    /// Gets the current stock status based on the available quantity.
    /// </summary>
    public StockStatus Status { get; private set; }


    // Private constructor required by ORM frameworks for entity materialization.
    private Inventory()
    {
        
    }


    /// <summary>
    /// Creates a new inventory instance for a product.
    /// </summary>
    /// <param name="productId">The identifier of the product.</param>
    /// <param name="quantity">The initial stock quantity.</param>
    public Inventory(Guid productId, StockQuantity quantity)
    {
        ProductId = productId;
        Quantity = quantity;
        Status = CalculateStockStatus(quantity);
    }


    /// <summary>
    /// Increases the available stock quantity.
    /// </summary>
    /// <param name="amount">The amount to add to the stock.</param>
    public void IncreaseStock(byte amount)
    {
        // Prevents increasing stock with an invalid amount.
        if (amount == 0)
            throw new Exception("Increase amount must be greater than zero.");

        // Updates the stock quantity with the increased amount.
        Quantity = StockQuantity.Create(
            (byte)(Quantity.Value + amount)
        );

        // Recalculates the stock status after the quantity update.
        Status = CalculateStockStatus(Quantity);
    }
    

    /// <summary>
    /// Creates and validates a new inventory aggregate.
    /// </summary>
    /// <param name="productId">The identifier of the product.</param>
    /// <param name="quantity">The initial stock quantity.</param>
    /// <returns>A new Inventory instance.</returns>
    public static Inventory Create(Guid productId, StockQuantity quantity)
    {
        // Ensures that inventory is always linked to a valid product.
        if (productId == Guid.Empty)
            throw new Exception("ProductId cannot be empty.");

        return new Inventory(productId, quantity);
    }
    

    /// <summary>
    /// Decreases the available stock quantity.
    /// </summary>
    /// <param name="amount">The amount to remove from the stock.</param>
    public void DecreaseStock(byte amount)
    {
        // Prevents decreasing stock with an invalid amount.
        if (amount == 0)
            throw new Exception("Decrease amount must be greater than zero.");

        // Prevents stock from becoming negative.
        if (amount > Quantity.Value)
            throw new Exception("Insufficient stock.");

        // Updates the stock quantity after decreasing the amount.
        Quantity = StockQuantity.Create(
            (byte)(Quantity.Value - amount)
        );

        // Recalculates the stock status after the quantity update.
        Status = CalculateStockStatus(Quantity);
    }


    /// <summary>
    /// Calculates the current stock status based on the available quantity.
    /// </summary>
    /// <param name="quantity">The current stock quantity.</param>
    /// <returns>The corresponding stock status.</returns>
    private static StockStatus CalculateStockStatus(StockQuantity quantity)
    {
        // No available items means the product is out of stock.
        if (quantity.Value == 0)
            return StockStatus.OutOfStock;

        // Five or fewer items are considered low stock.
        if (quantity.Value <= 5)
            return StockStatus.LowStock;

        // Any quantity above the low stock threshold is considered available.
        return StockStatus.InStock;
    }
}