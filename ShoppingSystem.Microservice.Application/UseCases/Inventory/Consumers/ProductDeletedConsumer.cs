using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Events;
using ShoppingSystem.Microservice.Application.Services;

namespace ShoppingSystem.Microservice.Application.UseCases.Inventory.Consumers;

public class ProductDeletedConsumer
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<ProductDeletedConsumer> _logger;

    public ProductDeletedConsumer(
        IInventoryService inventoryService,
        ILogger<ProductDeletedConsumer> logger)
    {
        _inventoryService = inventoryService;
        _logger = logger;
    }

    public async Task Handle(ProductDeletedIntegrationEvent message)
    {
        _logger.LogInformation(
            "Deleting inventory for ProductId: {ProductId}",
            message.ProductId);

        var inventoryResult = await _inventoryService
            .GetByProductIdAsync(message.ProductId, CancellationToken.None);

        if (!inventoryResult.Succeeded || inventoryResult.Data is null)
        {
            _logger.LogWarning(
                "Inventory not found for ProductId: {ProductId}",
                message.ProductId);

            return;
        }

        await _inventoryService.DeleteAsync(
            inventoryResult.Data,
            CancellationToken.None);

        _logger.LogInformation(
            "Inventory deleted successfully for ProductId: {ProductId}",
            message.ProductId);
    }
}