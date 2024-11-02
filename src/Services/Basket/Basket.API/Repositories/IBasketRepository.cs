namespace Basket.API.Repositories;

public interface IBasketRepository
{
    Task<Cart> GetCart(string userId, CancellationToken cancellationToken = default);
    Task<Cart> StoreCart(Cart cart, CancellationToken cancellationToken = default);
    Task<bool> DeleteCart(string userId, CancellationToken cancellationToken = default);
}