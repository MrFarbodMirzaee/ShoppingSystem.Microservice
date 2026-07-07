using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Events;
using ShoppingSystem.Microservice.Application.QueueNames;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.Product;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Notifications.Integrations;

public class ProductDeleteIntegrationHandler
    : INotificationHandler<ProductDeletedEvent>
{
    private readonly IMessagePublisherService _publisher;
    private readonly ILogger<ProductDeleteIntegrationHandler> _logger;

    public ProductDeleteIntegrationHandler(
        IMessagePublisherService publisher,
        ILogger<ProductDeleteIntegrationHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(
        ProductDeletedEvent notification,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Publishing ProductDeletedIntegrationEvent for ProductId: {ProductId}",
            notification.ProductId);

        var integrationEvent = new ProductDeletedIntegrationEvent(
            notification.ProductId);

        await _publisher.PublishAsync(
            integrationEvent,
            QueueName.ProductDeleted,
            ct);

        _logger.LogInformation(
            "ProductDeletedIntegrationEvent published successfully for ProductId: {ProductId}",
            notification.ProductId);
    }
}