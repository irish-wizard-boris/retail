using System;
using System.Threading.Tasks;
using Abysalto.Retail.Modules.Cart.Contracts.Requests;
using Abysalto.Retail.Modules.Cart.Application.DTO;

namespace Abysalto.Retail.Modules.Cart.Application.Services;

public interface ICartService
{
    Task<CartDto> AddItemToCartAsync(Guid cartId, CartItemDto cartItemDto);
}
