using AutoMapper;
using Carting.BLL.Exceptions;
using Carting.BLL.Models;
using Carting.BLL.Services.Interfaces;
using Carting.DAL.Repositories.Interfaces;
using FluentValidation;

namespace Carting.BLL.Services.Implementations;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CartItem> _cartItemValidator;

    public CartService(ICartRepository cartRepository, IMapper mapper, IValidator<CartItem> cartItemValidator)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _cartItemValidator = cartItemValidator;
    }
    public async Task<Cart> GetCartByExternalIdAsync(int cartExternalId)
    {
        var cart = await _cartRepository.GetCartByExternalIdAsync(cartExternalId);
        return _mapper.Map<Cart>(cart);
    }

    public Task CreateCartAsync(Cart cart)
    {
        var mapped = _mapper.Map<DAL.Models.Cart>(cart);
        return _cartRepository.CreateAsync(mapped);
    }

    public Task UpdateCartItemsAsync(CartItem cartItem)
    {
        var mapped = _mapper.Map<DAL.Models.CartItem>(cartItem);
        return _cartRepository.UpdateCartItemsAsync(mapped);
    }

    public async Task AddCartItemAsync(int cartExternalId, CartItem cartItem)
    {
        await _cartItemValidator.ValidateAndThrowAsync(cartItem);
        var existingCart = await _cartRepository.GetCartByExternalIdAsync(cartExternalId);
        var mappedCartItem = _mapper.Map<DAL.Models.CartItem>(cartItem);
        if (existingCart == null)
        {
            var newCart = new DAL.Models.Cart() { ExternalId = cartExternalId, Items = new [] { mappedCartItem }};
            await _cartRepository.CreateAsync(newCart);
        }
        else
        {
            await _cartRepository.AddCartItemAsync(cartExternalId, mappedCartItem);
        }
    }

    public async Task RemoveCartItemAsync(int cartExternalId, int cartItemExternalId)
    {
        if ((await _cartRepository.GetCartByExternalIdAsync(cartExternalId)) == null)
        {
            throw new NotFoundException();
        }
        await _cartRepository.RemoveCartItemAsync(cartExternalId, cartItemExternalId);
    }
}