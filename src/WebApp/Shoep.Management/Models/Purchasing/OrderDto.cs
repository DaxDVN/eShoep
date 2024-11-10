using Shoep.Management.Enum;

namespace Shoep.Management.Models.Purchasing;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    string OrderName,
    AddressDto ShippingAddress,
    AddressDto BillingAddress,
    PaymentDto Payment,
    OrderStatus Status,
    List<OrderItemDto> OrderItems,
    decimal TotalPrice);

public record AddressDto(
    string FirstName,
    string LastName,
    string EmailAddress,
    string AddressLine,
    string Country,
    string State,
    string ZipCode);

public record PaymentDto(string CardName, string CardNumber, string Expiration, string Cvv, int PaymentMethod);

public record OrderItemDto(Guid OrderId, Guid ProductId, string ProductName, int Quantity, decimal Price);