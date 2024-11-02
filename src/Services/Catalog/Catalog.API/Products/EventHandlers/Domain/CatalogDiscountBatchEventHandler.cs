using Catalog.API.Events;
using Catalog.API.Products.Command.BatchProduct;

namespace Catalog.API.Products.EventHandlers.Domain;

public class CatalogDiscountBatchEventHandler(ISender sender, ILogger<CatalogDiscountBatchEventHandler> logger)
    : INotificationHandler<CatalogDiscountBatchEvent>
{
    public async Task Handle(CatalogDiscountBatchEvent domainEvent, CancellationToken cancellationToken)
    {
        await sender.Send(new BatchProductCommand());
    }
}