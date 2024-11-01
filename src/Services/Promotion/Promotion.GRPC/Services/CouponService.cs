using Grpc.Core;
using MediatR;
using Promotion.API;
using Promotion.Application.Exceptions;
using Promotion.Application.Handlers.Coupons;

namespace Promotion.GRPC.Services;

public class CouponService(ILogger<CouponService> logger, ISender sender)
    : CouponProtoService.CouponProtoServiceBase
{
    public override async Task<CouponModel> ApplyCoupon(ApplyCouponRequest request, ServerCallContext context)
    {
        try
        {
            var command = new ApplyCouponCommand(Guid.Parse(request.UserId), Guid.Parse(request.CouponId));
            var result = await sender.Send(command, context.CancellationToken);

            logger.LogInformation("Coupon retrieved for UserId: {productId}, Amount: {amount}",
                request.UserId, result.Coupon.Amount);

            return new CouponModel
            {
                Id = result.Coupon.Id.ToString(),
                Code = result.Coupon.Code,
                CouponType = result.Coupon.CouponType.ToString(),
                Description = result.Coupon.Description,
                Amount = result.Coupon.Amount
            };
        }
        catch (CouponNotFoundException)
        {
            return new CouponModel
            {
                Id = "00000000-0000-0000-0000-000000000000",
                Code = "No Coupon Desc",
                CouponType = "No Coupon Desc",
                Description = "No Coupon Desc",
                Amount = 0
            };
        }
    }
}