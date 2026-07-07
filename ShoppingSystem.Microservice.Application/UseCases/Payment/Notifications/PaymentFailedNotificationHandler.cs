using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.Payment;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Notifications;

public class PaymentFailedNotificationHandler
    : INotificationHandler<PaymentFailedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IUserQueryService _userQueryService;

    public PaymentFailedNotificationHandler(
        IEmailService emailService,
        IUserQueryService userQueryService
        )
    {
        _emailService = emailService;
        _userQueryService = userQueryService;
    }

    public async Task Handle(PaymentFailedEvent notification, CancellationToken ct)
    {
        var email = await _userQueryService
            .GetEmailByIdAsync(notification.UserId, ct);
        
        if (email is null)
            throw new NullReferenceException(nameof(email));
        
        var body = $@"
            <h2>Payment Failed</h2>
            <p>We were unable to process your payment.</p>
            <p><b>Order ID:</b> {notification.OrderId}</p>
            <p><b>Reason:</b> {notification.Reason}</p>
            <p>Please try again or use a different payment method.</p>
        ";

        await _emailService.SendAsync(
            new EmailMessage
            {
                To = email,
                Subject = "Payment Failed",
                Body = body,
                IsHtml = true
            },
            ct);
    }
}