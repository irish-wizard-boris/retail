using System;
using System.Text.Json.Serialization;

namespace Abysalto.Retail.Modules.Cart.Contracts.Responses;

public class CartItemResponse
{
    [JsonPropertyName("Id")]
    public Guid Id { get; set; }

    [JsonPropertyName("productId")]
    public Guid ProductId { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("unitPrice")]
    public decimal UnitPrice { get; set; }

    [JsonPropertyName("totalPrice")]
    public decimal TotalPrice { get; set; }

    [JsonPropertyName("addedAt")]
    public DateTime AddedAt { get; set; }
}