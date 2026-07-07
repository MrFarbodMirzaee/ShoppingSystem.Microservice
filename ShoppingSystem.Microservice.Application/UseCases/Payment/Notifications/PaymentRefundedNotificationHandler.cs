using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.Payment;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Notifications;

public class PaymentRefundedNotificationHandler
    : INotificationHandler<PaymentRefundedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IUserQueryService _userQueryService;

    public PaymentRefundedNotificationHandler(
        IEmailService emailService,
        IUserQueryService userQueryService
    )
    {
        _emailService = emailService;
        _userQueryService = userQueryService;
    }

    public async Task Handle(PaymentRefundedEvent notification, CancellationToken ct)
    {
        var email = await _userQueryService
            .GetEmailByIdAsync(notification.UserId /* adjust if needed */, ct);
        
        if (email is null)
        throw new NullReferenceException();
        
        var body = $@"
            <h2>Payment Refunded</h2>
            <p>Your payment has been successfully refunded.</p>

            <p><b>Order ID:</b> {notification.OrderId}</p>
            <p><b>Amount:</b> {notification.Amount}</p>
            <p><b>Reason:</b> {notification.Message}</p>
            <p><b>Refunded At:</b> {notification.RefundedAt}</p>
        ";

        await _emailService.SendAsync(
            new EmailMessage
            {
                To = email, 
                Subject = "Payment Refunded",
                Body = body,
                IsHtml = true
            },
            ct);
    }
}