namespace Common.Messaging.Events;

public record DiscountCreatedEvent : IntegrationEvent
{
    public Guid DiscountId { get; set; }
    public required string PromotionType { get; init; }
    public decimal Amount { get; init; }
    public List<Guid> ProductIds { get; set; } = [];
}