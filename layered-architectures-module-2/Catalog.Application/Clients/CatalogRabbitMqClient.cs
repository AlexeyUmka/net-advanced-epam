using System.Text;
using System.Text.Json;
using Messaging.RabbitMq.Client;
using Messaging.RabbitMq.Client.Configuration;
using RabbitMQ.Client;

namespace Catalog.Application.Clients;

public class CatalogRabbitMqClient : IRabbitMqClient
{
    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly IModel _channel;

    public CatalogRabbitMqClient(IConnection rabbitMqConnection, RabbitMqConfig rabbitMqConfig)
    {
        _rabbitMqConfig = rabbitMqConfig;
        _channel = rabbitMqConnection.CreateModel();
        DeclareCatalogProductUpdateQueue();
    }

    public void Publish<TMessage>(TMessage message, string queueName) where TMessage : class
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _channel.BasicPublish(exchange: string.Empty,
            routingKey: queueName,
            basicProperties: null,
            body: body);
    }

    public void StartConsuming<TMessage>(string queueName, Func<TMessage, Task> action) where TMessage : class
    {
        throw new NotSupportedException();
    }

    private void DeclareCatalogProductUpdateQueue()
    {
        var queueConfig = _rabbitMqConfig.ProductUpdatedQueueConfig;
        if (queueConfig.HasDLQ)
        {
            _channel.QueueDeclare(queue: $"{queueConfig.Name}-DLQ",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
        _channel.QueueDeclare(queue: queueConfig.Name,
            durable: queueConfig.IsDurable,
            exclusive: queueConfig.IsExclusive,
            autoDelete: queueConfig.IsAutoDelete,
            arguments: queueConfig.Arguments);
    }
}