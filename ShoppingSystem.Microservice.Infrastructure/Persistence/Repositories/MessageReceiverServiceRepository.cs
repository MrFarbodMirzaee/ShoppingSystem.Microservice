using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShoppingSystem.Microservice.Application.Services;

namespace Infrastructure.Persistence.Repositories;

public class MessageReceiverServiceRepository
(IConnection connection) : IMessageReceiverService
{
    public async Task ReceiveAsync<T>
        (string queueName, Func<T, Task> handler,CancellationToken ct)
    {
        var channel = await connection
            .CreateChannelAsync(cancellationToken: ct);
        
        await channel
        .QueueDeclareAsync(queueName, durable: true
        , exclusive: false, autoDelete: false, cancellationToken: ct);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        
        consumer.ReceivedAsync += async (sender, args) =>
        {
            var json = Encoding.UTF8.GetString(args.Body.ToArray());

            var message = JsonConvert.DeserializeObject<T>(json);

            if (message is not null)
            {
                await handler(message);
            }

            await channel.BasicAckAsync(
                args.DeliveryTag,
                false,
                ct);
        };
        
        await channel
        .BasicConsumeAsync(queue: queueName, 
        autoAck: false, consumer: consumer, cancellationToken: ct);
        
        await Task.Delay(Timeout.Infinite, ct);
    }
}