using System;
using System.Threading.Tasks;
using Abysalto.Retail.Modules.Cart.Contracts.Requests;
using Abysalto.Retail.Modules.Cart.Application.DTO;

namespace Abysalto.Retail.Modules.Cart.Application.Services;

public interface ICartService
{
    Task<List<CartDto>> GetAllCartsAsync();
	Task<CartDto> GetCartByCustomerIdAsync(Guid customerId);
	Task<CartDto> AddItemToCartAsync(Guid customerId, CartItemDto cartItemDto);
	Task<CartItemDto> UpdateItemInCartAsync(Guid customerId, CartItemDto cartItemDto);
	Task DeleteAllCartItemsAsync(Guid customerId);
	Task DeleteSingleCartItemAsync(Guid customerId, Guid productId);
}
