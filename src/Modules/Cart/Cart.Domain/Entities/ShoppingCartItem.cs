using Ardalis.SharedKernel;

namespace Abysalto.Retail.Modules.Cart.Domain.Entities;

public class ShoppingCartItem : EntityBase<Guid>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime AddedAt { get; set; }
    public decimal TotalPrice { get; set; }
    public ShoppingCart Cart { get; set; } = null!;

    public ShoppingCartItem()
    {
        Id = Guid.NewGuid();
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
        {
            throw new CartDomainException("Quantity must be greater than zero.");
        }

        Quantity = newQuantity;
        RecalculateTotalPrice();
    }

    public void ItemAdded()
    {
        AddedAt = DateTime.UtcNow;
        RecalculateTotalPrice();
    }

    public decimal RecalculateTotalPrice() => TotalPrice = UnitPrice * Quantity;
}