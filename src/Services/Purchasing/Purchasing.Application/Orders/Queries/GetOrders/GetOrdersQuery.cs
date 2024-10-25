using Common.Pagination;

namespace Purchasing.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery(PaginationRequest PaginationRequest)
    : IQuery<GetOrdersResult>;

public record GetOrdersResult(PaginatedResult<OrderDto> Orders);