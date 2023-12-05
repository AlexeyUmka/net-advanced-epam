using AutoMapper;
using Carting.BLL.Models;
using Carting.BLL.Services.Interfaces;
using Grpc.Core;

namespace Carting.gRPC.Services;

public class CartingService : Carting.CartingBase
{
    private readonly ILogger<CartingService> _logger;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public CartingService(ILogger<CartingService> logger, ICartService cartService, IMapper mapper)
    {
        _logger = logger;
        _cartService = cartService;
        _mapper = mapper;
    }

    public override async Task<CartResponse> GetCartByExternalIdAsync(CartByExternalIdRequest request, ServerCallContext context)
    {
        var result = _mapper.Map<CartResponse>(await _cartService.GetCartByExternalIdAsync(request.CartExternalId));
        return result;
    }

    public override async Task GetCartItemsByExternalId(CartByExternalIdRequest request, IServerStreamWriter<CartItemResponse> responseStream,
        ServerCallContext context)
    {
        var cart = _mapper.Map<CartResponse>(await _cartService.GetCartByExternalIdAsync(request.CartExternalId));

        foreach (var item in cart.Items)
        {
            await responseStream.WriteAsync(item, context.CancellationToken);
        }
    }

    public override async Task<CartResponse> AddItemToCart(IAsyncStreamReader<AddCartItemRequest> requestStream, ServerCallContext context)
    {
        var lastCartId = 0;
        while (await requestStream.MoveNext(context.CancellationToken))
        {
            var requestItem = requestStream.Current;
            var cartItem = MapFromAddCartItemRequest(requestItem);
            await _cartService.AddCartItemAsync(requestItem.CartExternalId, cartItem);
            lastCartId = requestItem.CartExternalId;
        }
        
        var result = _mapper.Map<CartResponse>(await _cartService.GetCartByExternalIdAsync(lastCartId));
        return result;
    }

    public override async Task AddItemToCartBiDirectional(IAsyncStreamReader<AddCartItemRequest> requestStream, IServerStreamWriter<CartResponse> responseStream,
        ServerCallContext context)
    {
        while (await requestStream.MoveNext(context.CancellationToken))
        {
            var requestItem = requestStream.Current;
            var cartItem = MapFromAddCartItemRequest(requestItem);
            await _cartService.AddCartItemAsync(requestItem.CartExternalId, cartItem);
            var result = _mapper.Map<CartResponse>(await _cartService.GetCartByExternalIdAsync(requestItem.CartExternalId));
            await responseStream.WriteAsync(result, context.CancellationToken);
        }
    }

    private CartItem MapFromAddCartItemRequest(AddCartItemRequest requestItem)
    {
        return new CartItem()
        {
            Name = requestItem.ItemName,
            Price = (decimal)requestItem.ItemPrice,
            ExternalId = requestItem.ItemExternalId,
            Quantity = requestItem.ItemQuantity,
            Image = new BLL.Models.Image() { AltText = requestItem.ItemAltText, Url = requestItem.ItemUrl }
        };
    }
}