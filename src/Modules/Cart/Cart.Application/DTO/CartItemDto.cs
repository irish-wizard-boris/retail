using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Abysalto.Retail.Modules.Cart.Application.DTO;

public record CartItemDto
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CartId { get; set; }

    public CartDto Cart { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public DateTime AddedAt { get; set; }

    public decimal TotalPrice { get; set; }
}
