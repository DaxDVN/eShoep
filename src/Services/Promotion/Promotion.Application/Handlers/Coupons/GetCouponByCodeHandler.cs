namespace Promotion.Application.Handlers.Coupons;

public record GetCouponByCodeQuery(string UserId, string Code) : IQuery<GetCouponByCodeResult>;

public record GetCouponByCodeResult(CouponDto Coupon);

internal class GetCouponByCodeQueryHandler(IDocumentSession session)
    : IQueryHandler<GetCouponByCodeQuery, GetCouponByCodeResult>
{
    public async Task<GetCouponByCodeResult> Handle(GetCouponByCodeQuery query, CancellationToken cancellationToken)
    {
        var coupon = await session.Query<Coupon>().FirstOrDefaultAsync(
            c => (c.RedemptionCount < c.MaxRedemptions || c.MaxRedemptions == 0)
                 && (c.UserIds.Contains(Guid.Parse(query.UserId)) || c.UserIds.Count == 0)
                 && c.ExpirationDate > DateTime.Today && c.IsActive == true
                 && c.Code == query.Code, cancellationToken);

        if (coupon is null) throw new Exception("Coupon code not found");
        var isExist = await session.Query<CouponUsage>().AnyAsync(
            u => u.CouponId == coupon.Id
                 && u.UserId == Guid.Parse(query.UserId), cancellationToken);

        if (isExist) throw new Exception("Coupon is already used");

        var couponDto = coupon.Adapt<CouponDto>();
        return new GetCouponByCodeResult(couponDto);
    }
}