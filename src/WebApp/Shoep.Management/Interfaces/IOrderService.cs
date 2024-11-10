using Refit;
using Shoep.Management.Models.Purchasing;

namespace Shoep.Management.Interfaces;

public interface IOrderService
{
    [Get("/purchasing-service/orders/customer/{customerId}")]
    Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(string customerId);

    [Get("/purchasing-service/orders")]
    Task<GetOrdersResponse> GetOrders(PaginationRequest request);
}

public record GetOrdersResponse(PaginatedResult<OrderModel> Orders);
public record PaginationRequest(int PageIndex = 1, int PageSize = 10);
