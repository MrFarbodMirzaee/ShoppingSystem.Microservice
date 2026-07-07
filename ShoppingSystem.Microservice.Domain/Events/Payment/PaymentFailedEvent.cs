using MediatR;
using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.Events.Payment;

/// <summary>
/// Represents a domain event that occurs when a payment fails.
/// This event contains payment failure information and can be published
/// to notify other parts of the system about the failed payment.
/// </summary>
public class PaymentFailedEvent : DomainEvent, INotification
{
    /// <summary>
    /// Initializes a new instance of the PaymentFailedEvent class with user information.
    /// </summary>
    /// <param name="paymentId">The identifier of the failed payment.</param>
    /// <param name="orderId">The identifier of the related order.</param>
    /// <param name="userId">The identifier of the user associated with the payment.</param>
    /// <param name="reason">The reason why the payment failed.</param>
    public PaymentFailedEvent(
        Guid paymentId,
        Guid orderId,
        Guid userId,
        string reason)
    {
        // Ensures that a payment failure event always contains a valid reason.
        if (string.IsNullOrWhiteSpace(reason))
            throw new Exception("Payment failure reason cannot be empty.");

        PaymentId = paymentId;
        OrderId = orderId;
        Reason = reason;
        FailedAt = DateTimeOffset.UtcNow;
        UserId = userId;
    }
    

    /// <summary>
    /// Initializes a new instance of the PaymentFailedEvent class.
    /// </summary>
    /// <param name="paymentId">The identifier of the failed payment.</param>
    /// <param name="orderId">The identifier of the related order.</param>
    /// <param name="reason">The reason why the payment failed.</param>
    public PaymentFailedEvent(
        Guid paymentId,
        Guid orderId,
        string reason)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        Reason = reason;
        FailedAt = DateTimeOffset.UtcNow;
    }


    /// <summary>
    /// Gets the identifier of the failed payment.
    /// </summary>
    public Guid PaymentId { get; }


    /// <summary>
    /// Gets the identifier of the related order.
    /// </summary>
    public Guid OrderId { get; }


    /// <summary>
    /// Gets the reason why the payment failed.
    /// </summary>
    public string Reason { get; }


    /// <summary>
    /// Gets or sets the identifier of the user associated with the payment.
    /// </summary>
    public Guid UserId { get; set; }


    /// <summary>
    /// Gets the date and time when the payment failure occurred.
    /// </summary>
    public DateTimeOffset FailedAt { get; }

}