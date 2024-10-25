using Purchasing.Domain.Abstractions;

namespace Purchasing.Domain.Events
{
  public record OrderUpdatedEvent(Order order) : IDomainEvent;
}
