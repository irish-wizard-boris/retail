using System.Collections.Generic;
using System.Threading.Tasks;
using Abysalto.Retail.Modules.Cart.Domain.Entities;

namespace Abysalto.Retail.Modules.Cart.Domain.Repositories;

public interface ICartRepository
{
	Task<ShoppingCart?> GetByCustomerIdAsync(Guid customerId);
	Task<ShoppingCart?> GetByIdAsync(Guid id);
	Task<List<ShoppingCart>> GetAllAsync();
	Task AddAsync(ShoppingCart cart);
	Task UpdateAsync();
	Task<ShoppingCartItem> GetCartItemByIdAndCustomerAsync(Guid customerId, Guid productId);
	Task DeleteSingleCartItemAsync(Guid customerId, Guid productId);
	Task DeleteAllCartItemsAsync(Guid customerId);
}