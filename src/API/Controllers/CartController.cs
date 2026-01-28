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
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public CartController(ICartService cartService, IMapper mapper)
    {
        _cartService = cartService;
        _mapper = mapper;
    }

    [HttpGet("{cartId}")]
    public async Task<IActionResult> GetCartAsync(Guid cartId)
    {
        // Implementation for retrieving a cart by its ID
        return Ok();
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItemToCartAsync([FromQuery] Guid userId, [FromBody] AddItemToCartRequest cartItemRequest)
    {
        var cartItemDto = _mapper.Map<AddItemToCartRequest, CartItemDto>(cartItemRequest);
        var cartDto = await _cartService.AddItemToCartAsync(userId, cartItemDto);
        var response = _mapper.Map<CartDto, CartResponse>(cartDto);
		return Ok(response);
    }

    [HttpPut("items")]
    public async Task<IActionResult> UpdateItemInCartAsync(Guid cartId, [FromBody] UpdateCartItemRequest cartItemRequest)
    {
        // Implementation for adding an item to a cart
        return Ok();
    }

    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> RemoveItemFromCartAsync(Guid cartId, Guid cartItemId)
    {
        // Implementation for removing an item from a cart
        return Ok();
    }

    [HttpDelete("items")]
    public async Task<IActionResult> ClearCartAsync(Guid cartId)
    {
        return NoContent();
    }
}