using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abysalto.Retail.Modules.Cart.Domain.Entities;
using Abysalto.Retail.Modules.Cart.Domain.Repositories;
using Abysalto.Retail.Modules.Cart.Infrastructure.Data;

namespace AbysaltoRetail.Modules.Cart.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly CartDbContext _context;

    public CartRepository(CartDbContext context)
    {
        _context = context;
    }

    public async Task<ShoppingCart?> GetByCustomerIdAsync(Guid customerId)
    {
        var cart = await _context.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);
        if (cart != null) 
        {
            cart.RecalculateTotals(); 
        }
        return cart;
    }

	public async Task AddAsync(ShoppingCart cart)
	{
		await _context.Carts.AddAsync(cart);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync()
	{
		await _context.SaveChangesAsync();
	}

    public async Task<ShoppingCart?> GetByIdAsync(Guid id)
    {
        return await _context.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<ShoppingCart>> GetAllAsync()
    {
        var carts = await _context.Carts
            .Include(x => x.Items)
            .ToListAsync();
        carts.ForEach(c => c.RecalculateTotals());
        return carts;
    }

    public async Task<ShoppingCartItem?> GetCartItemByIdAndCustomerAsync(Guid customerId, Guid productId)
    {
		return await _context.CartItems
		    .FirstOrDefaultAsync(item =>
			    item.ProductId == productId &&
			    item.Cart.CustomerId == customerId);
	}

    public async Task DeleteSingleCartItemAsync(Guid customerId, Guid productId)
    {
        var cart = await _context.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

        if (cart == null) throw new CartDomainException("Cart not found.");

		var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (itemToRemove != null)
        {
            cart.Items.Remove(itemToRemove);
            cart.RecalculateTotals();
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllCartItemsAsync(Guid customerId)
    {
		var cart = await _context.Carts
			.Include(x => x.Items)
			.FirstOrDefaultAsync(x => x.CustomerId == customerId);

		if (cart == null) throw new CartDomainException("Cart not found.");

        cart.Items.Clear();
		await _context.SaveChangesAsync();
	}
}