using Refit;
using Shoep.Management.Models.Purchasing;

namespace Shoep.Management.Interfaces;

public interface IOrderService
{
    [Get("/purchasing-service/orders")]
    Task<GetOrdersResponse> GetOrders(PaginationRequest request);

    [Get("/purchasing-service/orders/{id}")]
    Task<GetOrderByIdResponse> GetOrderById(Guid id);

    [Put("/purchasing-service/orders/status")]
    Task<UpdateOrderStatusResponse> UpdateOrderStatus(UpdateOrderStatusRequest request);
}

public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public record PaginationRequest(int PageIndex = 1, int PageSize = 10);

public record GetOrderByIdResponse(OrderDto Order);

public record UpdateOrderStatusRequest(Guid OrderId, string NewStatus);

public record UpdateOrderStatusResponse(bool IsSuccess);