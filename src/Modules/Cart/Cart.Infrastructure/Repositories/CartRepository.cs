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
        return await _context.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.CustomerId == customerId); 
    }

	public async Task AddAsync(ShoppingCart cart)
	{
		await _context.Carts.AddAsync(cart);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(ShoppingCart cart)
	{
		_context.Carts.Update(cart);
		await _context.SaveChangesAsync();
	}
}