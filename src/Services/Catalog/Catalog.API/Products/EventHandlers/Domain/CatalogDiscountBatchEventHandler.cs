using Catalog.API.Events;

namespace Catalog.API.Products.EventHandlers.Domain;

public class CatalogDiscountBatchEventHandler(ILogger<CatalogDiscountBatchEventHandler> logger)
    : INotificationHandler<CatalogDiscountBatchEvent>
{
    public Task Handle(CatalogDiscountBatchEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}