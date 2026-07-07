using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.Order;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Notifications;

public class OrderCreatedNotificationHandler
    : INotificationHandler<OrderCreatedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IUserQueryService _userQueryService;

    public OrderCreatedNotificationHandler(
        IEmailService emailService,
        IUserQueryService userQueryService)
    {
        _emailService = emailService;
        _userQueryService = userQueryService;
    }

    public async Task Handle(OrderCreatedEvent notification, CancellationToken ct)
    {
        var email = await _userQueryService
            .GetEmailByIdAsync(notification.CustomerId, ct);

        if (email is null)
            throw new NullReferenceException(nameof(email));

        var body = $@"
            <h2>Order Created</h2>
            <p>Thank you for placing your order!</p>

            <p><b>Order ID:</b> {notification.OrderNumber}</p>
            <p><b>Created At:</b> {notification.CreatedAt}</p>
        ";

        await _emailService.SendAsync(
            new EmailMessage
            {
                To = email,
                Subject = "Order Created",
                Body = body,
                IsHtml = true
            },
            ct);
    }
}