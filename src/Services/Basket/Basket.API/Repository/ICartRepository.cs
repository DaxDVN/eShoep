namespace Basket.API.Data;

public interface ICartRepository
{
    Task<Cart> GetCart(string userId, CancellationToken cancellationToken = default);
    Task<Cart> StoreCart(Cart cart, CancellationToken cancellationToken = default);
    Task<bool> DeleteCart(string userId, CancellationToken cancellationToken = default);
}