using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Abysalto.Retail.Modules.Cart.Contracts.Requests;

public class UpdateItemInCartRequest
{
    [JsonPropertyName("productId")]
    public Guid ProductId { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
