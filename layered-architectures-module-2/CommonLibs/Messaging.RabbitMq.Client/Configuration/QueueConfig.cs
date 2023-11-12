namespace Messaging.RabbitMq.Client.Configuration;

public class QueueConfig
{
    public string Name { get; set; }
    public bool IsDurable { get; set; }
    public bool IsExclusive { get; set; }
    public bool IsAutoDelete { get; set; }
    public IDictionary<string, object> Arguments { get; set; }
    public bool HasDLQ { get; set; }
}