using Common.Exceptions;

namespace Promotion.API.Exceptions;

public class CouponNotFoundException : NotFoundException
{
    public CouponNotFoundException(Guid Id) : base("Coupon", Id)
    {
    }
}