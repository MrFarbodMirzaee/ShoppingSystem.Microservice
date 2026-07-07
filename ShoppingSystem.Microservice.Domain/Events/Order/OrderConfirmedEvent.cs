using MediatR;
using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Events.Order;

/// <summary>
/// Represents a domain event that occurs when an order is confirmed.
/// This event can be published through MediatR to notify other parts of the system
/// about the order confirmation.
/// </summary>
public class OrderConfirmedEvent : DomainEvent , INotification
{
    /// <summary>
    /// Initializes a new instance of the OrderConfirmedEvent class.
    /// </summary>
    /// <param name="customerId">The identifier of the customer who owns the order.</param>
    /// <param name="orderNumber">The unique number of the confirmed order.</param>
    public OrderConfirmedEvent(Guid customerId, OrderNumber orderNumber)
    {
        CustomerId = customerId;
        OrderNumber = orderNumber;
        ConfirmedAt = DateTimeOffset.UtcNow;
    }


    /// <summary>
    /// Gets the identifier of the customer who owns the confirmed order.
    /// </summary>
    public Guid CustomerId { get; }


    /// <summary>
    /// Gets the unique number of the confirmed order.
    /// </summary>
    public OrderNumber OrderNumber { get; }


    /// <summary>
    /// Gets the date and time when the order was confirmed.
    /// </summary>
    public DateTimeOffset ConfirmedAt { get; }
}