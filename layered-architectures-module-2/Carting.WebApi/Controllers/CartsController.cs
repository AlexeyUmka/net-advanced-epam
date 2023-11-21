using Asp.Versioning;
using AutoMapper;
using Carting.BLL.Services.Interfaces;
using Carting.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carting.WebApi.Controllers;

/// <summary>
/// CartsController
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/carts")]
[Authorize]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;
    public CartsController(ICartService cartService, IMapper mapper)
    {
        _cartService = cartService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Get cart and items by cartExternalId
    /// </summary>
    /// <param name="cartExternalId"></param>
    /// <returns></returns>
    [HttpGet("{cartExternalId:int}/items")]
    public async Task<Cart> GetCart(int cartExternalId)
    {
        var user = User;
        return _mapper.Map<Cart>(await _cartService.GetCartByExternalIdAsync(cartExternalId));
    }

    [HttpPost("{cartExternalId:int}/items")]
    public async Task<IActionResult> AddCartItem(int cartExternalId, CartItem cartItem)
    {
        var mappedCartItem = _mapper.Map<Carting.BLL.Models.CartItem>(cartItem);
        await _cartService.AddCartItemAsync(cartExternalId, mappedCartItem);
        return Ok();
    }
    
    [HttpDelete("{cartExternalId:int}/items/{cartItemExternalId:int}")]
    public async Task<IActionResult> RemoveCartItem(int cartExternalId, int cartItemExternalId)
    {
        await _cartService.RemoveCartItemAsync(cartExternalId, cartItemExternalId);
        return Ok();
    }
}