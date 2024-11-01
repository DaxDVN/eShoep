using Common.Exceptions;
using Grpc.Core;
using Mapster;
using Marten;
using MediatR;
using Promotion.API;
using Promotion.Application.Exceptions;
using Promotion.Application.Handlers.Coupons;
using Promotion.Domain.Models;

namespace Promotion.GRPC.Services;

public class CouponService(ILogger<CouponService> logger, ISender sender)
    : CouponProtoService.CouponProtoServiceBase
{
    // public override async Task<CouponModel> GetCoupon(GetCouponRequest request, ServerCallContext context)
    // {
    //     try
    //     {
    //         var query = new GetCouponByProductQuery(Guid.Parse(request.ProductId));
    //         var result = await sender.Send(query, context.CancellationToken);
    //
    //         logger.LogInformation("Coupon retrieved for ProductId: {productId}, Amount: {amount}",
    //             request.ProductId, result.Coupon.Amount);
    //
    //         return new CouponModel
    //         {
    //             ProductId = request.ProductId,
    //             Description = result.Coupon.Description,
    //             CouponType = result.Coupon.CouponType.ToString(),
    //             Amount = result.Coupon.Amount,
    //             Code = result.Coupon.Code,
    //             Id = result.Coupon.Id.ToString()
    //         };
    //     }
    //     catch (CouponNotFoundException)
    //     {
    //         return new CouponModel
    //         {
    //             ProductId = "00000000-0000-0000-0000-000000000000",
    //             Amount = 0,
    //             Description = "No Coupon Desc"
    //         };
    //     }
    // }
}