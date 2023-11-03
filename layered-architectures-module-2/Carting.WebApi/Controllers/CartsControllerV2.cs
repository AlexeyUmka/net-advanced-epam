using Asp.Versioning;
using AutoMapper;
using Carting.BLL.Services.Interfaces;
using Carting.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carting.WebApi.Controllers;

/// <summary>
/// CartsControllerV2
/// </summary>
[ApiController]
[ApiVersion("2")]
[Route("api/v{version:apiVersion}/carts")]
public class CartsControllerV2 : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;
    public CartsControllerV2(ICartService cartService, IMapper mapper)
    {
        _cartService = cartService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get items by cartExternalId
    /// </summary>
    /// <param name="cartExternalId"></param>
    /// <returns></returns>
    [HttpGet("{cartExternalId:int}/items")]
    public async Task<IEnumerable<CartItem>> GetItems(int cartExternalId)
    {
        return _mapper.Map<IEnumerable<CartItem>>((await _cartService.GetCartByExternalIdAsync(cartExternalId)).Items);
    }
}