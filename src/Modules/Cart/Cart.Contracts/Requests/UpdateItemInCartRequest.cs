using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Abysalto.Retail.Modules.Cart.Contracts.Requests;

public class UpdateCartItemRequest
{
    [Required(ErrorMessage = "Product ID is required")]
    [JsonPropertyName("productId")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be at least 0")]
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
