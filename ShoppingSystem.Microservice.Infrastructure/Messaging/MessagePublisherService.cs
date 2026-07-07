using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using ShoppingSystem.Microservice.Application.Attributes;
using ShoppingSystem.Microservice.Application.Services;

namespace Infrastructure.Messaging;

[ScopedService]
public class MessagePublisherService(IConnection connection) 
: IMessagePublisherService
{
    public async Task PublishAsync<T>
    (T message, string queueName,CancellationToken ct)
    {
        await using var channel = await connection
            .CreateChannelAsync(cancellationToken: ct);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false, cancellationToken: ct);

        var json = JsonConvert
            .SerializeObject(message);

        var body = Encoding
            .UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: string.Empty, 
            routingKey: queueName,
            body: body, cancellationToken: ct);
    }
}