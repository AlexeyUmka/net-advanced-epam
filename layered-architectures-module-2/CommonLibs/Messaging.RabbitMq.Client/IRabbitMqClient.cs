namespace Messaging.RabbitMq.Client;

public interface IRabbitMqClient
{
    public void Publish<TMessage>(TMessage message, string queueName) where TMessage : class;
    public void StartConsuming<TMessage>(string queueName, Func<TMessage, Task> action) where TMessage : class;
}