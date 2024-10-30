namespace Basket.API.Carts.GetCart;

public record GetCartQuery(string UserId) : IQuery<GetCartResult>;

public record GetCartResult(Cart Cart);

public class GetCartQueryHandler(ICartRepository repository) : IQueryHandler<GetCartQuery, GetCartResult>
{
    public async Task<GetCartResult> Handle(GetCartQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetCart(query.UserId);

        return new GetCartResult(basket);
    }
}