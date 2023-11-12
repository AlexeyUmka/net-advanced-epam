namespace Messaging.RabbitMq.Client.Messages;

public class ProductUpdatedMessage
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public decimal Price { get; set; }

    public uint Amount { get; set; }
}