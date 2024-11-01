using Common.Exceptions;

namespace Promotion.Application.Exceptions;

public class CouponNotFoundException : NotFoundException
{
    public CouponNotFoundException(Guid Id) : base("Coupon", Id)
    {
    }
}