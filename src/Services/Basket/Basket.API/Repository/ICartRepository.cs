namespace Basket.API.Data
{
    public interface ICartRepository
    {
        Task<Cart> GetCart(string userName, CancellationToken cancellationToken = default);
        Task<Cart> StoreCart(Cart cart, CancellationToken cancellationToken = default);
        Task<bool> DeleteCart(string userName, CancellationToken cancellationToken = default);
    }
}