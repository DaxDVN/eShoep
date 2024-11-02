using Common.Messaging.Events;
using MassTransit;

namespace Catalog.API.Products.EventHandlers.Integration;

public class DiscountCreatedEventHandler(IDocumentSession session, ILogger<DiscountCreatedEventHandler> logger)
    : IConsumer<DiscountCreatedEvent>
{
    public async Task Consume(ConsumeContext<DiscountCreatedEvent> context)
    {
        var discount = context.Message;

        var productsToUpdate = await session.Query<Product>()
            .Where(p => discount.ProductIds.Contains(p.Id))
            .ToListAsync();

        var productDiscounts = new List<ProductDiscount>();

        foreach (var product in productsToUpdate)
        {
            product.DiscountedPrice = discount.PromotionType == "FixedAmount"
                ? product.Price - discount.Amount
                : product.Price * (1 - discount.Amount / 100);
            if (product.DiscountedPrice < product.Price * 7 / 10) product.DiscountedPrice = product.Price * 7 / 10;
            productDiscounts.Add(new ProductDiscount
            {
                Id = Guid.NewGuid(),
                DiscountId = discount.Id,
                ProductId = product.Id,
                ExpirationDate = discount.ExpirationDate,
                IsActive = true
            });
        }

        session.Store<ProductDiscount>(productDiscounts);
        session.Update(productsToUpdate.ToArray());
        await session.SaveChangesAsync();
    }
}