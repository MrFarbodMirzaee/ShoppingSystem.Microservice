using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Events;
using ShoppingSystem.Microservice.Application.QueueNames;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.Product;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Notifications.Integrations;

public class ProductCreatedIntegrationHandler
    : INotificationHandler<ProductCreatedEvent>
{
    private readonly IMessagePublisherService _publisher;
    private readonly ILogger<ProductCreatedIntegrationHandler> _logger;

    public ProductCreatedIntegrationHandler(
        IMessagePublisherService publisher,
        ILogger<ProductCreatedIntegrationHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(
        ProductCreatedEvent notification,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Publishing ProductCreatedIntegrationEvent for ProductId: {ProductId}",
            notification.ProductId);

        var integrationEvent = new ProductCreatedIntegrationEvent(
            notification.ProductId,
            notification.Name.Value,
            notification.Price.Amount,
            notification.Price.Currency);

        await _publisher.PublishAsync(
            integrationEvent,
            QueueName.ProductCreated,
            ct);

        _logger.LogInformation(
            "ProductCreatedIntegrationEvent published successfully for ProductId: {ProductId}",
            notification.ProductId);
    }
}