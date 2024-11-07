using Refit;
using Shoep.Management.Models.Purchasing;

namespace Shoep.Management.Interfaces;

public interface IOrderService
{
    [Get("/purchasing-service/orders/customer/{customerId}")]
    Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(string customerId);
}