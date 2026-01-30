using System.Collections.Generic;
using System.Threading.Tasks;
using Abysalto.Retail.Modules.Cart.Application.DTO;
using Abysalto.Retail.Modules.Cart.Application.Services;
using Abysalto.Retail.Modules.Cart.Contracts.Requests;
using Abysalto.Retail.Modules.Cart.Contracts.Responses;
using API.Response;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Abysalto.Retail.API.Controllers
{
	/// <summary>
	/// Provides endpoints for managing customer carts.
	/// </summary>
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

		/// <summary>
		/// Retrieves the cart for a specific customer.
		/// <para>
		/// Note: In a real-world scenario, the customer ID would typically be obtained from a JWT token.
		/// </para>
		/// </summary>
		/// <param name="customerId">The unique identifier of the customer.</param>
		/// <returns>The cart with its associated items for the specified customer. Returns 404 if the cart does not exist.</returns>
		[HttpGet("{customerId}")]
		[ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(DomainErrorResponse), StatusCodes.Status404NotFound)]
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

		/// <summary>
		/// Adds an item to a customer's cart. If the customer does not have a cart, a new cart is created.
		/// <para> 
		/// Note: In a real-world scenario, the customer ID would typically be obtained from a JWT token.
		/// </para>
		/// </summary>
		/// <param name="customerId">The unique identifier of the customer.</param>
		/// <param name="cartItemRequest">The cart item to add, including product ID and quantity.</param>
		/// <returns>The updated cart with all associated items for the specified customer.</returns>
		[HttpPost("items")]
		[ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> AddItemToCartAsync([FromQuery] Guid customerId, [FromBody] AddItemToCartRequest cartItemRequest)
		{
			var cartItemDto = _mapper.Map<AddItemToCartRequest, CartItemDto>(cartItemRequest);
			var cartDto = await _cartService.AddItemToCartAsync(customerId, cartItemDto);
			var response = _mapper.Map<CartDto, CartResponse>(cartDto);
			return Ok(response);
		}

		/// <summary>
		/// Updates an item in a customer's cart.
		/// <para>
		/// Note: In a real-world scenario, the customer ID would typically be obtained from a JWT token.
		/// </para>
		/// </summary>
		/// <param name="customerId">The unique identifier of the customer.</param>
		/// <param name="cartItemRequest">The cart item to update, including product ID and quantity.</param>
		/// <returns>The updated cart item.</returns>
		[HttpPut("items")]
		[ProducesResponseType(typeof(CartItemResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(DomainErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateItemInCartAsync([FromQuery] Guid customerId, [FromBody] UpdateItemInCartRequest cartItemRequest)
		{
			var cartItemDto = _mapper.Map<UpdateItemInCartRequest, CartItemDto>(cartItemRequest);
			var response = await _cartService.UpdateItemInCartAsync(customerId, cartItemDto);
			return Ok(_mapper.Map<CartItemDto, CartItemResponse>(response));
		}

		/// <summary>
		/// Removes an item from a customer's cart.
		/// <para>
		/// Note: In a real-world scenario, the customer ID would typically be obtained from a JWT token.
		/// </para>
		/// </summary>
		/// <param name="customerId">The unique identifier of the customer.</param>
		/// <param name="productId">The identifier of the product to remove.</param>
		/// <returns>No content is returned.</returns>
		[HttpDelete("items/{productId}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(DomainErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> RemoveItemFromCartAsync([FromQuery] Guid customerId, Guid productId)
		{
			await _cartService.DeleteSingleCartItemAsync(customerId, productId);
			return NoContent();
		}

		/// <summary>
		/// Clears all items from a customer's cart.
		/// <para>
		/// Note: In a real-world scenario, the customer ID would typically be obtained from a JWT token.
		/// </para>
		/// </summary>
		/// <param name="customerId">The unique identifier of the customer.</param>
		/// <returns>No content is returned.</returns>
		[HttpDelete("items")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(DomainErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ClearCartAsync(Guid customerId)
		{
			await _cartService.DeleteAllCartItemsAsync(customerId);
			return NoContent();
		}
	}
}
