using Common.Exceptions;
using Discount.API.Models;
using Grpc.Core;
using Mapster;
using Marten;

namespace Discount.API.Services;

public class DiscountService(IDocumentSession session, ILogger<DiscountService> logger)
    : CouponProtoService.CouponProtoServiceBase
{
    public override async Task<CouponModel> GetCoupon(GetCouponRequest request, ServerCallContext context)
    {
        var coupon = await session.Query<Coupon>()
            .FirstOrDefaultAsync(x => x.ProductId == Guid.Parse(request.ProductId));
        if (coupon is null)
            coupon = new Coupon { ProductId = new Guid(), Amount = 0, Description = "No Coupon Desc" };
        logger.LogInformation("Coupon is retrieved for ProductName : {productName}, Amount : {amount}",
            coupon.ProductId, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
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

        if (!isExist)
        {
            throw new NotFoundException("Coupon does not exist.");
        }

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
            .FirstOrDefaultAsync(x => x.ProductId == Guid.Parse(request.ProductId));
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Coupon with ProductName={request.ProductId} is not found."));

        session.Delete<Coupon>(coupon);
        await session.SaveChangesAsync();

        logger.LogInformation("Coupon is successfully deleted. ProductName : {ProductName}", request.ProductId);

        return new DeleteCouponResponse { Success = true };
    }
}