namespace Basket.API.Data
{
    public class CartRepository(IDocumentSession session)
        : ICartRepository
    {
        public async Task<Cart> GetCart(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await session.LoadAsync<Cart>(userName, cancellationToken);

            return basket is null ? throw new CartNotFoundException(userName) : basket;
        }

        public async Task<Cart> StoreCart(Cart basket, CancellationToken cancellationToken = default)
        {
            session.Store(basket);
            await session.SaveChangesAsync(cancellationToken);
            return basket;
        }

        public async Task<bool> DeleteCart(string userName, CancellationToken cancellationToken = default)
        {
            session.Delete<Cart>(userName);
            await session.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}