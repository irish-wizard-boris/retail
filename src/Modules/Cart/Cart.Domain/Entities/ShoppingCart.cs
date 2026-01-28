using Ardalis.SharedKernel;
using Abysalto.Retail.Modules.Cart.Domain.Enums;

namespace Abysalto.Retail.Modules.Cart.Domain.Entities;

public class ShoppingCart : EntityBase<Guid>, IAggregateRoot
{
    public Guid CustomerId { get; set; }
    public CartStatus Status { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
	public decimal TotalAmount { get; set; }
	public int TotalItems { get; set; }

	public List<ShoppingCartItem> Items { get; set; } = new();

    public ShoppingCart()
    {
        Id = Guid.NewGuid();
    }

    public ShoppingCart(Guid customerId) : base()
    {
        CustomerId = customerId;
    }

    public decimal CalculateTotalAmount() => Items.Sum(item => item.UnitPrice * item.Quantity);

    public int CalculateTotalItems() => Items.Sum(item => item.Quantity);

    public void AddItem(ShoppingCartItem item)
    {
        var existingItem = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + item.Quantity);
        }
        else
        {
            item.ItemAdded();
            Items.Add(item);
        }

        RecalculateTotals();
		UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(Guid productId)
    {
        Items.RemoveAll(x => x.ProductId == productId);
		RecalculateTotals();
		UpdatedAt = DateTime.UtcNow;
    }

    public void Clear()
    {
        Items.Clear();
		RecalculateTotals();
		UpdatedAt = DateTime.UtcNow;
    }

    public void RecalculateTotals()
    {
        TotalAmount = CalculateTotalAmount();
        TotalItems = CalculateTotalItems();
    }

    public void ConvertToOrder()
    {
        // Implementation for converting cart to order goes here
    }
}