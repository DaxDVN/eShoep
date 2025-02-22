﻿namespace Basket.API.Repositories;

public class BasketRepository(IDocumentSession session)
    : IBasketRepository
{
    public async Task<Cart> GetCart(string userId, CancellationToken cancellationToken = default)
    {
        var basket = await session.Query<Cart>().FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        return basket is null ? throw new CartNotFoundException(userId) : basket;
    }

    public async Task<Cart> StoreCart(Cart basket, CancellationToken cancellationToken = default)
    {
        session.Store(basket);
        await session.SaveChangesAsync(cancellationToken);
        return basket;
    }

    public async Task<bool> DeleteCart(string userId, CancellationToken cancellationToken = default)
    {
        session.Delete<Cart>(userId);
        await session.SaveChangesAsync(cancellationToken);
        return true;
    }
}