namespace ShoppingSystem.Microservice.Domain.Enums;

/// <summary>
/// Defines the possible lifecycle states of an order.
/// Represents the current processing stage of a customer's order.
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// The order has been created but payment has not been completed.
    /// </summary>
    Pending,


    /// <summary>
    /// The order payment has been successfully completed.
    /// </summary>
    Paid,


    /// <summary>
    /// The order is being prepared for shipment.
    /// </summary>
    Preparing,


    /// <summary>
    /// The order has been shipped to the customer.
    /// </summary>
    Shipped,


    /// <summary>
    /// The order has been successfully delivered.
    /// </summary>
    Delivered,


    /// <summary>
    /// The order has been cancelled and will not be processed further.
    /// </summary>
    Cancelled
}