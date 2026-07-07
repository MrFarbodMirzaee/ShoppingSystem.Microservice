using MediatR;
using ShoppingSystem.Microservice.Domain.Common;

namespace ShoppingSystem.Microservice.Domain.Events.Payment;

/// <summary>
/// Represents a domain event that occurs when a payment is completed successfully.
/// This event contains payment information and can be published to notify
/// other parts of the system about the successful payment.
/// </summary>
public class PaymentSuccessfulEvent : DomainEvent , INotification
{
    /// <summary>
    /// Initializes a new instance of the PaymentSuccessfulEvent class with user information.
    /// </summary>
    /// <param name="paymentId">The identifier of the successful payment.</param>
    /// <param name="orderId">The identifier of the related order.</param>
    /// <param name="userId">The identifier of the user associated with the payment.</param>
    public PaymentSuccessfulEvent(Guid paymentId, Guid orderId,Guid userId)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        UserId = userId;
    }
    

    /// <summary>
    /// Initializes a new instance of the PaymentSuccessfulEvent class.
    /// </summary>
    /// <param name="paymentId">The identifier of the successful payment.</param>
    /// <param name="orderId">The identifier of the related order.</param>
    public PaymentSuccessfulEvent(Guid paymentId, Guid orderId)
    {
        PaymentId = paymentId;
        OrderId = orderId;
    }


    /// <summary>
    /// Gets the identifier of the successful payment.
    /// </summary>
    public Guid PaymentId { get; }


    /// <summary>
    /// Gets the identifier of the related order.
    /// </summary>
    public Guid OrderId { get; }


    /// <summary>
    /// Gets the identifier of the user associated with the payment.
    /// </summary>
    public Guid UserId { get; }
}