using Carting.DAL.Models;
using Carting.DAL.Repositories.Interfaces;
using MongoDB.Driver;

namespace Carting.DAL.Repositories.Implementations;

public class CartRepository : MongoRepositoryBase<Cart>, ICartRepository
{
    public CartRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    public virtual Task<Cart> GetCartByExternalIdAsync(int id)
    {
        return _itemCollection.FindAsync(Builders<Cart>.Filter.Eq(c => c.ExternalId, id)).Result.SingleOrDefaultAsync();
    }

    public virtual Task DeleteByExternalIdAsync(int id)
    {
        return _itemCollection.DeleteOneAsync(Builders<Cart>.Filter.Eq(c => c.ExternalId, id));
    }

    public async Task AddCartItemAsync(int cartExternalId, CartItem cartItem)
    {
        var cart = await _itemCollection.FindAsync(cart => cart.ExternalId == cartExternalId).Result
            .SingleOrDefaultAsync();
        cart.Items = cart.Items.Append(cartItem);
        await UpdateAsync(cart);
    }

    public async Task RemoveCartItemAsync(int cartExternalId, int cartItemExternalId)
    {
        var cart = await _itemCollection.FindAsync(cart => cart.ExternalId == cartExternalId).Result
            .SingleOrDefaultAsync();
        cart.Items = cart.Items.Where(item => item.ExternalId != cartItemExternalId);
        await UpdateAsync(cart);
    }
}