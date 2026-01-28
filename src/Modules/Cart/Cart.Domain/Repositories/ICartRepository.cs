using System.Collections.Generic;
using System.Threading.Tasks;
using Abysalto.Retail.Modules.Cart.Domain.Entities;

namespace Abysalto.Retail.Modules.Cart.Domain.Repositories;

public interface ICartRepository
{
	Task<ShoppingCart?> GetByCustomerIdAsync(Guid cartId);
	Task AddAsync(ShoppingCart cart);
	Task UpdateAsync(ShoppingCart cart);
}