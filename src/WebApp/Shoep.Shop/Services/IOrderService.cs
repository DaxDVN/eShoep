using Refit;
using Shoep.Shop.Models.Purchasing;

namespace Shoep.Shop.Services;

public interface IOrderService
{
    [Get("/purchasing-service/orders/customer/{customerId}")]
    Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(string customerId);
}