using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Abysalto.Retail.Mock;
using Abysalto.Retail.Modules.Cart.Application.DTO;
using Abysalto.Retail.Modules.Cart.Contracts.Requests;
using Abysalto.Retail.Modules.Cart.Domain.Entities;
using Abysalto.Retail.Modules.Cart.Domain.Exceptions;
using Abysalto.Retail.Modules.Cart.Domain.Repositories;
using AutoMapper;

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
            await _cartRepository.UpdateAsync();
        }

        return _mapper.Map<ShoppingCart, CartDto>(cart);
    }

    public async Task<List<CartDto>> GetAllCartsAsync()
    {
        var cartList = await _cartRepository.GetAllAsync();
        return cartList.Select(cart => _mapper.Map<ShoppingCart, CartDto>(cart)).ToList();
    }

    public async Task<CartDto> GetCartByIdAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetByIdAsync(cartId);
        return _mapper.Map<ShoppingCart, CartDto>(cart);
    }

	public async Task<CartDto> GetCartByCustomerIdAsync(Guid customerId)
	{
		var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
		return _mapper.Map<ShoppingCart, CartDto>(cart);
	}

    public async Task<CartItemDto> UpdateItemInCartAsync(Guid customerId, CartItemDto cartItemDto)
    {
        var cartItem = await _cartRepository.GetCartItemByIdAndCustomerAsync(customerId, cartItemDto.ProductId);
        if (cartItem == null)
        {
            throw new CartItemNotFound();
		}
        else
        {
            cartItem.UpdateQuantity(cartItemDto.Quantity);
            _cartRepository.UpdateAsync();
        }
        return _mapper.Map<ShoppingCartItem, CartItemDto>(cartItem);
    }

    public async Task DeleteAllCartItemsAsync(Guid customerId)
    {
		await _cartRepository.DeleteAllCartItemsAsync(customerId);
	}

    public async Task DeleteSingleCartItemAsync(Guid customerId, Guid productId)
    {
        await _cartRepository.DeleteSingleCartItemAsync(customerId, productId);
    }
}