using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CachedCartRepository(ICartRepository repository, IDistributedCache cache)
    : ICartRepository
{
    public async Task<Cart> GetCart(string userName, CancellationToken cancellationToken = default)
    {
        var cachedCart = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedCart))
            return JsonSerializer.Deserialize<Cart>(cachedCart)!;

        var basket = await repository.GetCart(userName, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<Cart> StoreCart(Cart cart, CancellationToken cancellationToken = default)
    {
        await repository.StoreCart(cart, cancellationToken);

        await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart), cancellationToken);

        return cart;
    }

    public async Task<bool> DeleteCart(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteCart(userName, cancellationToken);

        await cache.RemoveAsync(userName, cancellationToken);

        return true;
    }
}