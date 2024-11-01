using Common.Exceptions;

namespace Promotion.Application.Exceptions;

public class DiscountNotFoundException : NotFoundException
{
    public DiscountNotFoundException(Guid Id) : base("Discount", Id)
    {
    }
}