using Refit;
using Shoep.Management.Models.Promotion;

namespace Shoep.Management.Interfaces;

public interface IDiscountService
{
    [Get("/promotion-service/promotion/discounts")]
    Task<GeDiscountsResponse> GetDiscounts(int? pageNumber = 1, int? pageSize = 7);

    [Post("/promotion-service/promotion/discounts")]
    Task<CreateDiscountResponse> CreateDiscount([Body] CreateDiscountRequest request);

    [Put("/promotion-service/promotion/discounts/toggle-status")]
    Task<ToggleStatusDiscountResponse> ToggleStatusAsync([Body] ToggleStatusDiscountRequest request);
}

public record GeDiscountsResponse(IEnumerable<Discount> Discounts, long TotalDiscounts);

public record CreateDiscountResponse(Guid Id);

public record ToggleStatusDiscountResponse(bool IsSuccess);