using AutoMapper;
using Carting.BLL.Exceptions;
using Carting.BLL.Services.Interfaces;
using Messaging.RabbitMq.Client;
using Messaging.RabbitMq.Client.Configuration;
using Messaging.RabbitMq.Client.Messages;
using CartItem = Carting.BLL.Models.CartItem;

namespace Carting.WebApi.BackgroundServices;

public class MessageProcessingService : BackgroundService
{
    private readonly IRabbitMqClient _rabbitMqClient;
    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public MessageProcessingService(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        _rabbitMqClient = scope.ServiceProvider.GetRequiredService<IRabbitMqClient>();
        _rabbitMqConfig = scope.ServiceProvider.GetRequiredService<RabbitMqConfig>();
        _cartService = scope.ServiceProvider.GetRequiredService<ICartService>();
        _mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _rabbitMqClient.StartConsuming<ProductUpdatedMessage>(_rabbitMqConfig.ProductUpdatedQueueConfig.Name, async (message) =>
        {
            var cartItem = _mapper.Map<CartItem>(message);
            await _cartService.UpdateCartItemsAsync(cartItem);
        });
        while (stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }

        await Task.CompletedTask;
    }
}