using MediatR;
using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Events.Payment;

/// <summary>
/// Represents a domain event that occurs when a payment is refunded.
/// This event contains refund details and can be published to notify
/// other parts of the system about the refund operation.
/// </summary>
public class PaymentRefundedEvent : DomainEvent , INotification
{
    /// <summary>
    /// Initializes a new instance of the PaymentRefundedEvent class.
    /// </summary>
    /// <param name="paymentId">The identifier of the refunded payment.</param>
    /// <param name="userId">The identifier of the user associated with the payment.</param>
    /// <param name="amount">The refunded payment amount.</param>
    /// <param name="message">The refund reason or message.</param>
    /// <param name="orderId">The identifier of the related order.</param>
    public PaymentRefundedEvent(
        Guid paymentId,
        Guid userId,
        Money amount,
        string message,
        Guid orderId)
    {
        // Ensures that a refund event always contains a valid reason.
        if (string.IsNullOrWhiteSpace(message))
            throw new Exception("Refund reason cannot be empty.");

        PaymentId = paymentId;
        UserId = userId;
        Amount = amount;
        Message = message;
        RefundedAt = DateTimeOffset.UtcNow;
        OrderId = orderId;
    }


    /// <summary>
    /// Gets the identifier of the refunded payment.
    /// </summary>
    public Guid PaymentId { get; }


    /// <summary>
    /// Gets the identifier of the related order.
    /// </summary>
    public Guid OrderId { get; }


    /// <summary>
    /// Gets or sets the identifier of the user associated with the payment.
    /// </summary>
    public Guid UserId { get; set; }


    /// <summary>
    /// Gets the refunded payment amount.
    /// </summary>
    public Money Amount { get; }


    /// <summary>
    /// Gets the refund message or reason.
    /// </summary>
    public string Message { get; }


    /// <summary>
    /// Gets the date and time when the payment was refunded.
    /// </summary>
    public DateTimeOffset RefundedAt { get; }

}