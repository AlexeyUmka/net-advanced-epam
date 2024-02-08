using Carting.DAL.Models;

namespace Carting.DAL.Repositories.Interfaces;

public interface ICartRepository : IMongoRepository<Cart>
{
    Task<Cart?> GetCartByExternalIdAsync(int cartExternalId);
    Task AddCartItemAsync(int cartExternalId, CartItem cartItem);
    Task RemoveCartItemAsync(int cartExternalId, int cartItemExternalId);
    Task UpdateCartItemsAsync(CartItem cartItem);
}