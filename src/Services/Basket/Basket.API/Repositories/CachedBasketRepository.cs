using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache)
    : IBasketRepository
{
    public async Task<Cart> GetCart(string userId, CancellationToken cancellationToken = default)
    {
        var cachedCart = await cache.GetStringAsync(userId, cancellationToken);
        if (!string.IsNullOrEmpty(cachedCart))
            return JsonSerializer.Deserialize<Cart>(cachedCart)!;

        var cart = await repository.GetCart(userId, cancellationToken);
        await cache.SetStringAsync(userId, JsonSerializer.Serialize(cart), cancellationToken);
        return cart;
    }

    public async Task<Cart> StoreCart(Cart cart, CancellationToken cancellationToken = default)
    {
        await repository.StoreCart(cart, cancellationToken);

        await cache.SetStringAsync(cart.UserId, JsonSerializer.Serialize(cart), cancellationToken);

        return cart;
    }

    public async Task<bool> DeleteCart(string userId, CancellationToken cancellationToken = default)
    {
        await repository.DeleteCart(userId, cancellationToken);

        await cache.RemoveAsync(userId, cancellationToken);

        return true;
    }
}