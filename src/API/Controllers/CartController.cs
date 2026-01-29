using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abysalto.Retail.Modules.Cart.Contracts.Requests;
using Abysalto.Retail.Modules.Cart.Contracts.Responses;
using Abysalto.Retail.Modules.Cart.Application.Services;
using AutoMapper;
using Abysalto.Retail.Modules.Cart.Application.DTO;


namespace Abysalto.Retail.API.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public CartController(ICartService cartService, IMapper mapper)
    {
        _cartService = cartService;
        _mapper = mapper;
    }

	[HttpGet("{customerId}")]
	public async Task<IActionResult> GetCartByCustomerIdAsync(Guid customerId)
	{
		var cartDto = await _cartService.GetCartByCustomerIdAsync(customerId);
		if (cartDto == null)
		{
			return NotFound();
		}
		var response = _mapper.Map<CartDto, CartResponse>(cartDto);
		return Ok(response);
	}

	[HttpPost("items")]
    public async Task<IActionResult> AddItemToCartAsync([FromQuery] Guid customerId, [FromBody] AddItemToCartRequest cartItemRequest)
    {
        var cartItemDto = _mapper.Map<AddItemToCartRequest, CartItemDto>(cartItemRequest);
        var cartDto = await _cartService.AddItemToCartAsync(customerId, cartItemDto);
        var response = _mapper.Map<CartDto, CartResponse>(cartDto);
		return Ok(response);
    }

    [HttpPut("items")]
    public async Task<IActionResult> UpdateItemInCartAsync([FromQuery] Guid customerId, [FromBody] UpdateItemInCartRequest cartItemRequest)
    {
        var cartItemDto = _mapper.Map<UpdateItemInCartRequest, CartItemDto>(cartItemRequest);
        var response = await _cartService.UpdateItemInCartAsync(customerId, cartItemDto);
        return Ok(_mapper.Map<CartItemDto, CartItemResponse>(response));
    }

    [HttpDelete("items/{productId}")]
    public async Task<IActionResult> RemoveItemFromCartAsync([FromQuery] Guid customerId, Guid productId)
    {
		await _cartService.DeleteSingleCartItemAsync(customerId, productId);
		return NoContent();
    }

    [HttpDelete("items")]
    public async Task<IActionResult> ClearCartAsync(Guid customerId)
    {
        await _cartService.DeleteAllCartItemsAsync(customerId);
        return NoContent();
    }
}