using System.Text;
using System.Text.Json;
using Messaging.RabbitMq.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Carting.BLL.Clients;

public class CartingRabbitMqClient : IRabbitMqClient
{
    private readonly IModel _channel;

    public CartingRabbitMqClient(IConnection rabbitMqConnection)
    {
        _channel = rabbitMqConnection.CreateModel();
    }

    public void Publish<TMessage>(TMessage message, string queueName) where TMessage : class
    {
        throw new NotSupportedException();
    }

    public void StartConsuming<TMessage>(string queueName, Func<TMessage, Task> action) where TMessage : class
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (sender, args) =>
        {
            action(JsonSerializer.Deserialize<TMessage>(Encoding.UTF8.GetString(args.Body.ToArray())));
            _channel.BasicAck(args.DeliveryTag, false);
        };
        _channel.BasicConsume(queueName, autoAck: false, consumer);
    }
}