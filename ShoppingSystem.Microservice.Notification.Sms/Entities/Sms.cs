using ShoppingSystem.Microservice.Notification.Sms.Enums;

namespace ShoppingSystem.Microservice.Notification.Sms.Entities;

public class Sms
{
    public Guid Id { get; private set; }

    public string PhoneNumber { get; private set; }

    public string Message { get; private set; }

    public SmsStatus Status { get; private set; }

    public string? ProviderMessageId { get; private set; }

    public int RetryCount { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? SentAt { get; private set; }

    public string? ErrorMessage { get; private set; }

    private Sms() { } // EF Core

    public Sms(string phoneNumber, string message)
    {
        Id = Guid.NewGuid();
        PhoneNumber = phoneNumber;
        Message = message;
        Status = SmsStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsSent(string providerMessageId)
    {
        Status = SmsStatus.Sent;
        ProviderMessageId = providerMessageId;
        SentAt = DateTime.UtcNow;
    }
    
    public void MarkAsSending()
    {
        Status = SmsStatus.Sending;
    }

    public void MarkAsFailed(string error)
    {
        Status = SmsStatus.Failed;
        ErrorMessage = error;
        RetryCount++;
    }
}