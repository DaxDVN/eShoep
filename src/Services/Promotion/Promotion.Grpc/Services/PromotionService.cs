using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Promotion.Grpc.Data;
using Promotion.Grpc.Models;

namespace Promotion.Grpc.Services
{
  public class PromotionService(PromotionContext dbContext, ILogger<PromotionService> logger)
    : CouponProtoService.CouponProtoServiceBase
  {
    public override async Task<CouponModel> GetCoupon(GetCouponRequest request, ServerCallContext context)
    {
      try
      {
        var coupon = await dbContext
          .Coupons
          .FirstOrDefaultAsync(x => x.ProductId == Guid.Parse(request.ProductId));

        if (coupon is null)
          coupon = new Coupon { ProductId = new Guid(), Amount = 0, Description = "No Coupon Desc" };

        logger.LogInformation("Coupon is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductId, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
      }
      catch (Exception ex)
      {
        logger.LogError(ex.Message);
        return null;
      }
    }

    public override async Task<CouponModel> CreateCoupon(CreateCouponRequest request, ServerCallContext context)
    {
      var coupon = request.Coupon.Adapt<Coupon>();
      if (coupon is null)
        throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

      dbContext.Coupons.Add(coupon);
      await dbContext.SaveChangesAsync();

      logger.LogInformation("Coupon is successfully created. ProductName : {ProductName}", coupon.ProductId);

      var couponModel = coupon.Adapt<CouponModel>();
      return couponModel;
    }


    public override async Task<CouponModel> UpdateCoupon(UpdateCouponRequest request, ServerCallContext context)
    {
      var coupon = request.Coupon.Adapt<Coupon>();
      if (coupon is null)
        throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

      dbContext.Coupons.Update(coupon);
      await dbContext.SaveChangesAsync();

      logger.LogInformation("Coupon is successfully updated. ProductName : {ProductName}", coupon.ProductId);

      var couponModel = coupon.Adapt<CouponModel>();
      return couponModel;
    }

    public override async Task<DeleteCouponResponse> DeleteCoupon(DeleteCouponRequest request, ServerCallContext context)
    {
      var coupon = await dbContext
          .Coupons
          .FirstOrDefaultAsync(x => x.ProductId == Guid.Parse(request.ProductId));

      if (coupon is null)
        throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with ProductName={request.ProductId} is not found."));

      dbContext.Coupons.Remove(coupon);
      await dbContext.SaveChangesAsync();

      logger.LogInformation("Coupon is successfully deleted. ProductName : {ProductName}", request.ProductId);

      return new DeleteCouponResponse { Success = true };
    }
  }
}
