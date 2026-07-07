using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.Order;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Notifications;

public class OrderCancelledNotificationHandler
    : INotificationHandler<OrderCancelledEvent>
{
    private readonly IEmailService _emailService;
    private readonly IUserQueryService _userQueryService;

    public OrderCancelledNotificationHandler(
        IEmailService emailService,
        IUserQueryService userQueryService)
    {
        _emailService = emailService;
        _userQueryService = userQueryService;
    }

    public async Task Handle(
        OrderCancelledEvent notification,
        CancellationToken ct)
    {
        var email = await _userQueryService
            .GetEmailByIdAsync(notification.CustomerId, ct);

        if (email is null)
            return;

        var body = $@"
            <h2>Order Cancelled</h2>
            <p>Your order has been cancelled successfully.</p>

            <p><b>Order ID:</b> {notification.OrderNumber}</p>
            <p><b>Reason:</b> {notification.Reason}</p>
            <p><b>Cancelled At:</b> {notification.CancelledAt}</p>

            <br/>
            <p>If you have any questions, please contact our support team.</p>
        ";

        await _emailService.SendAsync(
            new EmailMessage
            {
                To = email,
                Subject = "Order Cancelled",
                Body = body,
                IsHtml = true
            },
            ct);
    }
}