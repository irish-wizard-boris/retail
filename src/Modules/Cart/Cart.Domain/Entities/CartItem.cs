using Ardalis.SharedKernel;

namespace Abysalto.Retail.Modules.Cart.Domain.Entities;

public class CartItem : EntityBase<Guid>
{
    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public DateTime AddedAt { get; private set; }
    public Cart Cart { get; private set; } = null!;

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
        {
            throw new CartDomainException("Quantity must be greater than zero.");
        }

        Quantity = newQuantity;
    }

    public void ItemAdded() => AddedAt = DateTime.UtcNow;

    public decimal CalculateTotal() => Price * Quantity;
}