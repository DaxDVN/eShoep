using Refit;
using Shoep.Management.Models.Purchasing;

namespace Shoep.Management.Interfaces;

public interface IOrderService
{
    [Get("/purchasing-service/orders")]
    Task<GetOrdersResponse> GetOrders(PaginationRequest request);

    [Get("/purchasing-service/orders/{id}")]
    Task<GetOrderByIdResponse> GetOrderById(Guid id);
}

public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public record PaginationRequest(int PageIndex = 1, int PageSize = 10);

public record GetOrderByIdResponse(OrderDto Order);