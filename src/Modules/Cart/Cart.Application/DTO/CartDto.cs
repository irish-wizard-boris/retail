using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Abysalto.Retail.Modules.Cart.Application.DTO;

public record CartDto
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public string Status { get; set; } = "Active";

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal TotalAmount {  get; set; }

    public int TotalItems {  get; set; }

	public virtual ICollection<CartItemDto> Items { get; set; } = new List<CartItemDto>();
}
