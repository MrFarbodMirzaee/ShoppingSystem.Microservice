using MediatR;
using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Events.Order;

/// <summary>
/// Represents a domain event that occurs when an order is cancelled.
/// This event contains cancellation details and can be published through MediatR
/// to notify other parts of the application.
/// </summary>
public class OrderCancelledEvent : DomainEvent , INotification
{
    /// <summary>
    /// Initializes a new instance of the OrderCancelledEvent class.
    /// </summary>
    /// <param name="orderNumber">The unique number of the cancelled order.</param>
    /// <param name="reason">The reason why the order was cancelled.</param>
    /// <param name="customerId">The identifier of the customer who owns the order.</param>
    public OrderCancelledEvent(
        OrderNumber orderNumber,
        string reason,
        Guid customerId)
    {
        // Ensures that a cancellation event always contains a valid reason.
        if (string.IsNullOrWhiteSpace(reason))
            throw new Exception("Cancellation reason cannot be empty.");

        OrderNumber = orderNumber;
        Reason = reason;
        CancelledAt = DateTimeOffset.UtcNow;
        CustomerId = customerId;
    }


    /// <summary>
    /// Gets the unique number of the cancelled order.
    /// </summary>
    public OrderNumber OrderNumber { get; }


    /// <summary>
    /// Gets the reason provided for the order cancellation.
    /// </summary>
    public string Reason { get; }


    /// <summary>
    /// Gets the date and time when the order was cancelled.
    /// </summary>
    public DateTimeOffset CancelledAt { get; }


    /// <summary>
    /// Gets the email information associated with the cancellation notification.
    /// </summary>
    public Email Email { get; }
    

    /// <summary>
    /// Gets the identifier of the customer who owns the cancelled order.
    /// </summary>
    public Guid CustomerId { get; }
}