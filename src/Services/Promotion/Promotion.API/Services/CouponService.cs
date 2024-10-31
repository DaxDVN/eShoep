using Common.Exceptions;
using Grpc.Core;

namespace Promotion.API.Services;

public class CouponService(IDocumentSession session, ILogger<CouponService> logger)
    : CouponProtoService.CouponProtoServiceBase
{
    public override async Task<CouponModel> GetCoupon(GetCouponRequest request, ServerCallContext context)
    {
        var coupon = await session.Query<Coupon>()
            .FirstOrDefaultAsync(x => x.ProductId.Contains(Guid.Parse(request.ProductId)));
        if (coupon is null)
            return new CouponModel
                { ProductId = "00000000-0000-0000-0000-000000000000", Amount = 0, Description = "No Coupon Desc" };
        logger.LogInformation("Coupon is retrieved for ProductName : {productName}, Amount : {amount}",
            coupon.ProductId, coupon.Amount);

        var couponModel = new CouponModel
        {
            ProductId = request.ProductId,
            Description = coupon.Description,
            CouponType = coupon.CouponType.ToString(),
            Amount = coupon.Amount
        };
        return couponModel;
    }

    public override async Task<CouponModel> CreateCoupon(CreateCouponRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
        session.Store(coupon);
        await session.SaveChangesAsync();

        logger.LogInformation("Coupon is successfully created. ProductName : {ProductName}", coupon.ProductId);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }


    public override async Task<CouponModel> UpdateCoupon(UpdateCouponRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        var isExist = await session.Query<Coupon>().AnyAsync(x => x.ProductId == coupon.ProductId);

        if (!isExist) throw new NotFoundException("Coupon does not exist.");

        session.Update(coupon);
        await session.SaveChangesAsync();

        logger.LogInformation("Coupon is successfully updated. ProductName : {ProductName}", coupon.ProductId);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteCouponResponse> DeleteCoupon(DeleteCouponRequest request,
        ServerCallContext context)
    {
        var coupon = await session.Query<Coupon>()
            .FirstOrDefaultAsync(x => x.ProductId.Contains(Guid.Parse(request.ProductId)));
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Coupon with ProductName={request.ProductId} is not found."));
        coupon.ProductId.Remove(Guid.Parse(request.ProductId));
        session.Update(coupon);
        await session.SaveChangesAsync();

        logger.LogInformation("Coupon is successfully deleted. ProductName : {ProductName}", request.ProductId);

        return new DeleteCouponResponse { Success = true };
    }
}