namespace ShoppingSystem.Microservice.Domain.Enums;

/// <summary>
/// Defines the possible lifecycle states of a payment.
/// Represents the current processing state of a payment transaction.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// The payment has been created but has not been completed yet.
    /// </summary>
    Pending,


    /// <summary>
    /// The payment has been successfully processed.
    /// </summary>
    Successful,


    /// <summary>
    /// The payment processing has failed.
    /// </summary>
    Failed,


    /// <summary>
    /// The payment has been refunded to the customer.
    /// </summary>
    Refunded
}