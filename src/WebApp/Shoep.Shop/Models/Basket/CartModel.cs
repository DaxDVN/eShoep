﻿namespace Shoep.Shop.Models.Basket;

public class CartModel
{
    public string UserId { get; set; } = default!;
    public List<CartItemModel> Items { get; set; } = [];
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
}

public class CartItemModel
{
    public int Quantity { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public Guid ProductId { get; set; } = default!;
    public string? ProductName { get; set; } = default!;
    public string? ImageUrl { get; set; } = default!;
}

public record GetCartResponse(CartModel Cart);

public record StoreCartRequest(CartModel Cart);

public record StoreCartResponse(string UserId);

public record DeleteCartResponse(bool IsSuccess);