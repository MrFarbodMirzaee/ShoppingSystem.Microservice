namespace ShoppingSystem.Microservice.Domain.Enums;

/// <summary>
/// Defines the possible states of product stock availability.
/// Represents the current inventory condition of a product.
/// </summary>
public enum StockStatus
{
    /// <summary>
    /// The product has no available stock.
    /// </summary>
    OutOfStock,


    /// <summary>
    /// The product stock level is below the defined threshold.
    /// </summary>
    LowStock,


    /// <summary>
    /// The product has sufficient stock available.
    /// </summary>
    InStock,


    /// <summary>
    /// The product stock has been temporarily reserved.
    /// </summary>
    Reserved,


    /// <summary>
    /// The product has been discontinued and is no longer available.
    /// </summary>
    Discontinued
}