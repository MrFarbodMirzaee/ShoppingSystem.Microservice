using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.Payment;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Notifications;

public class PaymentSuccessfulNotificationHandler
    : INotificationHandler<PaymentSuccessfulEvent>
{
    private readonly IEmailService _emailService;
    private readonly IUserQueryService _userQueryService;

    public PaymentSuccessfulNotificationHandler(
        IEmailService emailService,
        IUserQueryService userQueryService
    )
    {
        _emailService = emailService;
        _userQueryService = userQueryService;
    }

    public async Task Handle(PaymentSuccessfulEvent notification, CancellationToken ct)
    {
        
        var email = await _userQueryService
            .GetEmailByIdAsync(notification.UserId /* adjust if needed */, ct);
        
        var body = $@"
            <h2>Payment Successful</h2>
            <p>Thank you! Your payment was completed successfully.</p>

            <p><b>Order ID:</b> {notification.OrderId}</p>
            <p><b>Payment ID:</b> {notification.PaymentId}</p>

            <br/>
            <p>You will receive your order confirmation soon.</p>
        ";

        await _emailService.SendAsync(
            new EmailMessage
            {
                To = email,
                Subject = "Payment Successful",
                Body = body,
                IsHtml = true
            },
            ct);
    }
}