using MediatR;
using ShoppingSystem.Microservice.Application.QueueNames;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.Order;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Notifications.Integrations;

public class OrderCreatedIntegrationHandler
    : INotificationHandler<OrderCreatedEvent>
{
    private readonly IMessagePublisherService _publisher;

    public OrderCreatedIntegrationHandler(
        IMessagePublisherService publisher)
    {
        _publisher = publisher;
    }

    public async Task Handle(
        OrderCreatedEvent notification,
        CancellationToken ct)
    {
        var integrationEvent = new OrderCreatedEvent(
            notification.OrderNumber,
            notification.CustomerId,
            notification.TotalPrice,
            notification.Items);

        await _publisher.PublishAsync(
            integrationEvent,
            QueueName.OrderCreated,
            ct);
    }
}