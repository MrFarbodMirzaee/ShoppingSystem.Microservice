using ShoppingSystem.Microservice.Domain.Common;
using ShoppingSystem.Microservice.Domain.Enums;
using ShoppingSystem.Microservice.Domain.Events.Payment;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Domain.Aggregates.Payment;

/// <summary>
/// Represents the Payment aggregate root.
/// Responsible for managing payment lifecycle operations such as completion,
/// failure, and refund status changes.
/// </summary>
public class Payment : AggregateRoot
{
    // Private constructor required by ORM frameworks for entity materialization.
    private Payment()
    {
    }


    /// <summary>
    /// Creates a new payment instance for an order.
    /// </summary>
    /// <param name="orderId">The identifier of the related order.</param>
    /// <param name="amount">The payment amount.</param>
    /// <returns>A new Payment instance.</returns>
    public static Payment Create(
        Guid orderId,
        Money amount)
    {
        return new Payment
        {
            OrderId = orderId,
            Amount = amount,
            Status = PaymentStatus.Pending
        };
    }


    /// <summary>
    /// Completes the payment successfully and records the transaction information.
    /// </summary>
    /// <param name="transactionId">The external transaction identifier.</param>
    public void Complete(TransactionId transactionId)
    {
        // Stores the transaction identifier received from the payment provider.
        TransactionId = transactionId;

        // Updates payment state to successful.
        Status = PaymentStatus.Successful;

        // Records the payment completion time.
        PaidAt = DateTime.UtcNow;


        // Raises a domain event to notify other parts of the system.
        Raise(new PaymentSuccessfulEvent(Id , OrderId));
    }
    

    /// <summary>
    /// Marks the payment as failed without additional information.
    /// </summary>
    public void Fail()
    {
        Status = PaymentStatus.Failed;
    }
    

    /// <summary>
    /// Marks the payment as failed and records the failure reason.
    /// </summary>
    /// <param name="reason">The reason why the payment failed.</param>
    public void Fail(string reason)
    {
        Status = PaymentStatus.Failed;


        // Raises a domain event containing failure details.
        Raise(new PaymentFailedEvent(
            Id,
            OrderId,
            reason));
    }
    

    /// <summary>
    /// Refunds the payment by updating its status.
    /// </summary>
    public void Refund()
    {
        Status = PaymentStatus.Refunded;
    }
    

    /// <summary>
    /// Gets the identifier of the related order.
    /// </summary>
    public Guid OrderId { get; private set; }


    /// <summary>
    /// Navigation property representing the related order.
    /// </summary>
    public Order.Order Order { get; set; }


    /// <summary>
    /// Gets the payment amount.
    /// </summary>
    public Money Amount { get; private set; }


    /// <summary>
    /// Gets the transaction identifier returned by the payment provider.
    /// </summary>
    public TransactionId? TransactionId { get; private set; }


    /// <summary>
    /// Gets the current payment status.
    /// </summary>
    public PaymentStatus Status { get; private set; }


    /// <summary>
    /// Gets the date and time when the payment was completed.
    /// </summary>
    public DateTimeOffset? PaidAt { get; private set; }

}