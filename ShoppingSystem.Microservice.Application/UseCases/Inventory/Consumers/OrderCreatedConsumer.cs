using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Events;
using ShoppingSystem.Microservice.Application.Services;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Consumers;

public class OrderCreatedConsumer
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(
        IInventoryService inventoryService,
        ILogger<OrderCreatedConsumer> logger)
    {
        _inventoryService = inventoryService;
        _logger = logger;
    }

    public async Task Handle(OrderCreatedIntegrationEvent message)
    {
        _logger.LogInformation(
            "Processing OrderCreatedIntegrationEvent. OrderId: {OrderId}",
            message.OrderId);

        foreach (var item in message.Items)
        {
            _logger.LogInformation(
                "Decreasing stock. ProductId: {ProductId}, Quantity: {Quantity}",
                item.ProductId,
                item.Quantity);

            await _inventoryService.DecreaseStockAsync(
                item.ProductId,
                item.Quantity,
                CancellationToken.None);
        }

        _logger.LogInformation(
            "Inventory updated successfully for OrderId: {OrderId}",
            message.OrderId);
    }
}