using Purchasing.Domain.Abstractions;

namespace Purchasing.Domain.Events
{
  public record OrderCreatedEvent(Order order) : IDomainEvent;
}
