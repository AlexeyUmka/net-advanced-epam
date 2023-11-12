using Carting.BLL.Models;

namespace Carting.BLL.Services.Interfaces;

public interface ICartService
{
    Task<Cart> GetCartByExternalIdAsync(int cartExternalId);
    Task AddCartItemAsync(int cartExternalId, CartItem cartItem);
    Task RemoveCartItemAsync(int cartExternalId, int cartItemExternalId);
    Task UpdateCartItemsAsync(CartItem cartItem);
}