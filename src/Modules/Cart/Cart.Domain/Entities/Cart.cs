using Ardalis.SharedKernel;

namespace Abysalto.Retail.Modules.Cart.Domain.Entities;

public class Cart : EntityBase<Guid>, IAggregateRoot
{
    public Guid CustomerId { get; private set; }

    public CartStatus Status { get; private set; } 
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private List<CartItem> Items = new();

    public decimal CalculateTotal() => Items.Sum(item => item.Price * item.Quantity);

    public void AddItem(CartItem item)
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

        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(Guid productId)
    {
        Items.RemoveAll(x => x.ProductId == productId);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Clear()
    {
        Items.Clear();
        UpdatedAt = DateTime.UtcNow;
    }

    public void ConvertToOrder()
    {
        // Implementation for converting cart to order goes here
    }
}