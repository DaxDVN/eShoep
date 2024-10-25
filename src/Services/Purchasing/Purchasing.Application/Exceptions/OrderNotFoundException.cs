using Common.Exceptions;

namespace Purchasing.Application.Exceptions
{
  public class OrderNotFoundException : NotFoundException
  {
    public OrderNotFoundException(Guid id) : base("Order", id)
    {
    }
  }
}
