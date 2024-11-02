using Catalog.API.Products.Command.BatchProduct;
using Catalog.API.Repositories;
using Common.Messaging.Events;
using MassTransit;

namespace Catalog.API.Products.EventHandlers.Integration;

public class BatchCatalogEventHandler(
    ICatalogRepository repository,
    ISender sender,
    ILogger<DiscountCreatedEventHandler> logger)
    : IConsumer<CatalogBatchEvent>
{
    public async Task Consume(ConsumeContext<CatalogBatchEvent> context)
    {
        await repository.ToggleBatch();
        await sender.Send(new BatchProductCommand());
    }
}