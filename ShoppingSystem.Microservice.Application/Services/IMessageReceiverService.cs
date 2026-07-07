namespace ShoppingSystem.Microservice.Application.Services;

public interface IMessageReceiverService
{
    Task ReceiveAsync<T>
        (string queueName
        ,Func<T, Task> handler
        ,CancellationToken ct);
}