using AutoMapper;
using Catalog.Domain;
using MediatR;
using Messaging.RabbitMq.Client;
using Messaging.RabbitMq.Client.Configuration;

namespace Catalog.Application.Common.Behaviours;

public class DomainEventsBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IRabbitMqClient _rabbitMqClient;
    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly IMapper _mapper;

    public DomainEventsBehaviour(IRabbitMqClient rabbitMqClient, RabbitMqConfig rabbitMqConfig, IMapper mapper)
    {
        _rabbitMqClient = rabbitMqClient;
        _rabbitMqConfig = rabbitMqConfig;
        _mapper = mapper;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        ProductDomainEventsHandlers.Configure(_rabbitMqClient, _rabbitMqConfig, _mapper);

        return await next();
    }
}
