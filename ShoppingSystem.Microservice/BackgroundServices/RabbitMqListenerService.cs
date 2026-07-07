using ShoppingSystem.Microservice.Application.Events;
using ShoppingSystem.Microservice.Application.QueueNames;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Application.UseCases.Inventory.Consumers;

namespace ShoppingSystem.Microservice.BackgroundServices;

public class RabbitMqListenerService : BackgroundService
{
    private readonly IMessageReceiverService _receiver;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RabbitMqListenerService> _logger;

    public RabbitMqListenerService(
        IMessageReceiverService receiver,
        IServiceScopeFactory scopeFactory,
        ILogger<RabbitMqListenerService> logger)
    {
        _receiver = receiver;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogInformation("RabbitMQ listener service started.");

        ct.Register(() =>
        {
            _logger.LogInformation("RabbitMQ listener service is stopping.");
        });

        _logger.LogInformation(
            "Subscribing to queues: {Queues}",
            string.Join(", ",
                QueueName.ProductCreated,
                QueueName.ProductDeleted,
                QueueName.OrderCreated));

        var tasks = new[]
        {
            _receiver.ReceiveAsync<ProductCreatedIntegrationEvent>(
                QueueName.ProductCreated,
                async message =>
                {
                    try
                    {
                        _logger.LogInformation(
                            "Received ProductCreatedIntegrationEvent. ProductId: {ProductId}",
                            message.ProductId);

                        using var scope = _scopeFactory.CreateScope();

                        var consumer = scope.ServiceProvider
                            .GetRequiredService<ProductCreatedConsumer>();

                        await consumer.Handle(message);

                        _logger.LogInformation(
                            "Processed ProductCreatedIntegrationEvent successfully. ProductId: {ProductId}",
                            message.ProductId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Error processing ProductCreatedIntegrationEvent. ProductId: {ProductId}",
                            message.ProductId);

                        throw;
                    }
                },
                ct),

            _receiver.ReceiveAsync<ProductDeletedIntegrationEvent>(
                QueueName.ProductDeleted,
                async message =>
                {
                    try
                    {
                        _logger.LogInformation(
                            "Received ProductDeletedIntegrationEvent. ProductId: {ProductId}",
                            message.ProductId);

                        using var scope = _scopeFactory.CreateScope();

                        var consumer = scope.ServiceProvider
                            .GetRequiredService<ProductDeletedConsumer>();

                        await consumer.Handle(message);

                        _logger.LogInformation(
                            "Processed ProductDeletedIntegrationEvent successfully. ProductId: {ProductId}",
                            message.ProductId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Error processing ProductDeletedIntegrationEvent. ProductId: {ProductId}",
                            message.ProductId);

                        throw;
                    }
                },
                ct),

            _receiver.ReceiveAsync<OrderCreatedIntegrationEvent>(
                QueueName.OrderCreated,
                async message =>
                {
                    try
                    {
                        _logger.LogInformation(
                            "Received OrderCreatedIntegrationEvent. OrderId: {OrderId}",
                            message.OrderId);

                        using var scope = _scopeFactory.CreateScope();

                        var consumer = scope.ServiceProvider
                            .GetRequiredService<OrderCreatedConsumer>();

                        await consumer.Handle(message);

                        _logger.LogInformation(
                            "Processed OrderCreatedIntegrationEvent successfully. OrderId: {OrderId}",
                            message.OrderId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Error processing OrderCreatedIntegrationEvent. OrderId: {OrderId}",
                            message.OrderId);

                        throw;
                    }
                },
                ct)
        };

        return Task.WhenAll(tasks);
    }
}