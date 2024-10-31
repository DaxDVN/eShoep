using Promotion.API.Dtos;
using Promotion.API.Exceptions;

namespace Promotion.API.Coupons.GetCouponById;

public record GetCouponByIdQuery(Guid Id) : IQuery<GetCouponByIdResult>;

public record GetCouponByIdResult(CouponDto Coupon);

internal class GetCouponByIdQueryHandler(IDocumentSession session)
    : IQueryHandler<GetCouponByIdQuery, GetCouponByIdResult>
{
    public async Task<GetCouponByIdResult> Handle(GetCouponByIdQuery query, CancellationToken cancellationToken)
    {
        var coupon = await session.LoadAsync<Coupon>(query.Id, cancellationToken);
        if (coupon is null)
        {
            throw new CouponNotFoundException(query.Id);
        }

        var couponDto = coupon.Adapt<CouponDto>();
        return new GetCouponByIdResult(couponDto);
    }
}