using Basket.API.Repositories;
using Common.Messaging.Events;
using MassTransit;

namespace Basket.API.Carts.GetCart;

public record GetCartQuery(string UserId) : IQuery<GetCartResult>;

public record GetCartResult(Cart Cart);

public class GetCartQueryHandler(IPublishEndpoint publishEndpoint, IBasketRepository repository)
    : IQueryHandler<GetCartQuery, GetCartResult>
{
    public async Task<GetCartResult> Handle(GetCartQuery query, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new CatalogBatchEvent(), cancellationToken);
        var basket = await repository.GetCart(query.UserId, cancellationToken);
        return new GetCartResult(basket);
    }
}