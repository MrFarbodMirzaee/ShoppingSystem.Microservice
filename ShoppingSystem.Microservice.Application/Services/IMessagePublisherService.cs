namespace ShoppingSystem.Microservice.Application.Services;

public interface IMessagePublisherService
{
    Task PublishAsync<T>
        (T message
        , string queueName
        ,CancellationToken ct);
}