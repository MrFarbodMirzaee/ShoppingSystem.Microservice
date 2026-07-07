using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.Order;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Notifications;

public class OrderConfirmedNotificationHandler
    : INotificationHandler<OrderConfirmedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IUserQueryService _userQueryService;

    public OrderConfirmedNotificationHandler(
        IEmailService emailService,
        IUserQueryService userQueryService)
    {
        _emailService = emailService;
        _userQueryService = userQueryService;
    }

    public async Task Handle(
        OrderConfirmedEvent notification,
        CancellationToken ct)
    {
        var email = await _userQueryService
            .GetEmailByIdAsync(notification.CustomerId /* adjust if needed */, ct);

        if (email is null)
            return;

        var body = $@"
            <h2>Order Confirmed</h2>
            <p>Thank you for your purchase!</p>

            <p>Your order has been confirmed and is now being processed.</p>

            <p><b>Order ID:</b> {notification.OrderNumber}</p>
            <p><b>Confirmed At:</b> {notification.ConfirmedAt}</p>

            <br/>
            <p>We'll notify you again when your order has been shipped.</p>
        ";

        await _emailService.SendAsync(
            new EmailMessage
            {
                To = email,
                Subject = "Order Confirmed",
                Body = body,
                IsHtml = true
            },
            ct);
    }
}