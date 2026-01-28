using System;
using System.Threading.Tasks;
using Abysalto.Retail.Modules.Cart.Contracts.Requests;
using Abysalto.Retail.Modules.Cart.Application.DTO;
using Abysalto.Retail.Modules.Cart.Domain.Repositories;
using Abysalto.Retail.Modules.Cart.Domain.Entities;
using AutoMapper;
using Abysalto.Retail.Mock;

namespace Abysalto.Retail.Modules.Cart.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IPriceService _priceService;
    private readonly IMapper _mapper;

    public CartService(ICartRepository cartRepository, IPriceService priceService, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _priceService = priceService;
        _mapper = mapper;
    }

    public async Task<CartDto> AddItemToCartAsync(Guid customerId, CartItemDto cartItemDto) 
    {
        var cartItem = _mapper.Map<CartItemDto, ShoppingCartItem>(cartItemDto);
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId);

        cartItem.UnitPrice = _priceService.GetPrice(cartItemDto.ProductId);

		if (cart == null)
        {
            cart = new ShoppingCart(customerId);
            cart.AddItem(cartItem);

            await _cartRepository.AddAsync(cart);
		}
        else
        {
			cart.AddItem(cartItem);

			await _cartRepository.UpdateAsync(cart);
		}   

        return _mapper.Map<ShoppingCart, CartDto>(cart);
    }   
}