using AutoMapper;
using Carting.BLL.Services.Interfaces;
using Carting.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carting.WebApi.Controllers;

[ApiController]
[Route("api/carts")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;
    public CartController(ICartService cartService, IMapper mapper)
    {
        _cartService = cartService;
        _mapper = mapper;
    }
    [HttpGet("{cartExternalId:int}/items")]
    public async Task<Cart> Get(int cartExternalId)
    {
        return _mapper.Map<Cart>(await _cartService.GetCartByExternalIdAsync(cartExternalId));
    }
    
    [HttpPost("{cartExternalId:int}/items")]
    public async Task AddCartItem(int cartExternalId, CartItem cartItem)
    {
        var mappedCartItem = _mapper.Map<Carting.BLL.Models.CartItem>(cartItem);
        await _cartService.AddCartItemAsync(cartExternalId, mappedCartItem);
    }
    
    [HttpDelete("{cartExternalId:int}/items/{cartItemExternalId:int}")]
    public async Task RemoveCartItem(int cartExternalId, int cartItemExternalId)
    {
        await _cartService.RemoveCartItemAsync(cartExternalId, cartItemExternalId);
    }
}