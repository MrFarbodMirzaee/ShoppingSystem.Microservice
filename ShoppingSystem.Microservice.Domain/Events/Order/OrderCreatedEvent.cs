using MediatR;
using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Events.Order;

/// <summary>
/// Represents a domain event that occurs when a new order is created.
/// This event contains order information required by other parts of the system
/// for further processing, notifications, or integration workflows.
/// </summary>
public class OrderCreatedEvent : DomainEvent, INotification
{
    /// <summary>
    /// Initializes a new instance of the OrderCreatedEvent class with order items.
    /// </summary>
    /// <param name="orderNumber">The unique number of the created order.</param>
    /// <param name="customerId">The identifier of the customer who created the order.</param>
    /// <param name="totalPrice">The total price of the order.</param>
    /// <param name="items">The collection of items included in the order.</param>
    public OrderCreatedEvent(
        OrderNumber orderNumber,
        Guid customerId,
        Money totalPrice,
        List<OrderItem> items)
    {
        OrderNumber = orderNumber;
        CustomerId = customerId;
        TotalPrice = totalPrice;
        Items = items;
        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Initializes a new instance of the OrderCreatedEvent class without order items.
    /// </summary>
    /// <param name="orderNumber">The unique number of the created order.</param>
    /// <param name="customerId">The identifier of the customer who created the order.</param>
    /// <param name="totalPrice">The total price of the order.</param>
    public OrderCreatedEvent(
        OrderNumber orderNumber,
        Guid customerId,
        Money totalPrice)
    {
        OrderNumber = orderNumber;
        CustomerId = customerId;
        TotalPrice = totalPrice;
        CreatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Gets the unique number of the created order.
    /// </summary>
    public OrderNumber OrderNumber { get; }


    /// <summary>
    /// Gets the identifier of the customer who created the order.
    /// </summary>
    public Guid CustomerId { get; }


    /// <summary>
    /// Gets the total price of the created order.
    /// </summary>
    public Money TotalPrice { get; }


    /// <summary>
    /// Gets the collection of items included in the order.
    /// </summary>
    public List<OrderItem> Items { get; }


    /// <summary>
    /// Gets the date and time when the order was created.
    /// </summary>
    public DateTime CreatedAt { get; }


    /// <summary>
    /// Represents a simplified order item model used inside the order created event.
    /// Contains the product identifier and quantity information.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Gets or sets the identifier of the product.
        /// </summary>
        public Guid ProductId { get; set; }


        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public byte Quantity { get; set; }
    }
}