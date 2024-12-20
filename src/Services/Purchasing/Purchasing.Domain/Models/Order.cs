﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Purchasing.Domain.Models;

public class Order : Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = new();

    [NotMapped] private decimal _discountedTotalPrice;
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public CustomerId CustomerId { get; private set; } = default!;
    public OrderName OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public decimal TotalPrice
    {
        get => _discountedTotalPrice > 0 ? _discountedTotalPrice : OrderItems.Sum(x => x.Price * x.Quantity);
        private set => _discountedTotalPrice = value;
    }

    [NotMapped]
    public decimal DiscountedTotalPrice
    {
        get => _discountedTotalPrice > 0 ? _discountedTotalPrice : TotalPrice;
        set => _discountedTotalPrice = value;
    }

    public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress,
        Address billingAddress, Payment payment)
    {
        var order = new Order
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment,
            Status = OrderStatus.Pending
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment,
        OrderStatus status)
    {
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        Status = status;

        AddDomainEvent(new OrderUpdatedEvent(this));
    }

    public void Add(ProductId productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var orderItem = new OrderItem(Id, productId, quantity, price);
        _orderItems.Add(orderItem);
    }

    public void Remove(ProductId productId)
    {
        var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
        if (orderItem is not null) _orderItems.Remove(orderItem);
    }

    public void ApplyCoupon(decimal discountAmount)
    {
        if (discountAmount < 0 || discountAmount > TotalPrice)
            throw new ArgumentOutOfRangeException(nameof(discountAmount),
                "Discount amount must be positive and less than the total price.");

        _discountedTotalPrice = TotalPrice - discountAmount;
    }

    public void UpdateStatus(OrderStatus newStatus)
    {
        Status = newStatus;
    }
}