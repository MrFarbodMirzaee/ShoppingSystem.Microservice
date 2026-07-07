using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Events;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.ValueObjects;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Consumers;

public class ProductCreatedConsumer
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<ProductCreatedConsumer> _logger;

    public ProductCreatedConsumer(
        IInventoryService inventoryService,
        ILogger<ProductCreatedConsumer> logger)
    {
        _inventoryService = inventoryService;
        _logger = logger;
    }

    public async Task Handle(ProductCreatedIntegrationEvent message)
    {
        _logger.LogInformation(
            "Creating inventory for ProductId: {ProductId}",
            message.ProductId);

        var inventory = Domain.Aggregates.Inventory.Inventory.Create(
            message.ProductId,
            StockQuantity.Create(0));

        await _inventoryService.AddAsync(inventory, CancellationToken.None);

        _logger.LogInformation(
            "Inventory created successfully for ProductId: {ProductId}",
            message.ProductId);
    }
}