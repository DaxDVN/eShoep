namespace Basket.API.Basket.GetCart
{
    public record GetCartQuery(string UserName) : IQuery<GetCartResult>;

    public record GetCartResult(Cart Cart);

    public class GetCartQueryHandler(ICartRepository repository) : IQueryHandler<GetCartQuery, GetCartResult>
    {
        public async Task<GetCartResult> Handle(GetCartQuery query, CancellationToken cancellationToken)
        {
            var basket = await repository.GetCart(query.UserName);

            return new GetCartResult(basket);
        }
    }
}