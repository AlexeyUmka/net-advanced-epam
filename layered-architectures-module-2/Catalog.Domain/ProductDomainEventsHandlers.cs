using AutoMapper;
using Catalog.Domain.Entities;
using Messaging.RabbitMq.Client;
using Messaging.RabbitMq.Client.Configuration;
using Messaging.RabbitMq.Client.Messages;

namespace Catalog.Domain;

public static class ProductDomainEventsHandlers
{
    private static IRabbitMqClient _rabbitMq;
    private static RabbitMqConfig _rabbitMqConfig;
    private static IMapper _mapper;

    public static void Configure(IRabbitMqClient rabbitMqClient, RabbitMqConfig rabbitMqConfig, IMapper mapper)
    {
        _rabbitMq = rabbitMqClient;
        _rabbitMqConfig = rabbitMqConfig;
        _mapper = mapper;
    }
    public static void ProductUpdatedHandler(object sender, EventArgs eventArgs)
    {
        var product = _mapper.Map<ProductUpdatedMessage>(sender as Product ?? throw new ArgumentException("Sender must not be empty"));
        _rabbitMq.Publish(product, _rabbitMqConfig.ProductUpdatedQueueConfig.Name);
    }
}