using Abysalto.Retail.Modules.Cart.Application.DTO;
using Abysalto.Retail.Modules.Cart.Application.Services;
using Abysalto.Retail.Modules.Cart.Contracts.Responses;
using API.Response;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Abysalto.Retail.API.Controllers
{
	/// <summary>
	/// Provides endpoints for managing admin operations on customer carts.
	/// </summary>
	[ApiController]
	[Route("api/admin/cart")]
	public class CartAdminController : Controller
	{
		private readonly ICartService _cartService;
		private readonly IMapper _mapper;

		public CartAdminController(ICartService cartService, IMapper mapper)
		{
			_cartService = cartService;
			_mapper = mapper;
		}

		/// <summary>
		/// Retrieves all customer carts along with their items.
		/// </summary>
		/// <returns>A list of all customer carts with their associated items.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllCartsAsync()
		{
			var cartDtoList = await _cartService.GetAllCartsAsync();
			var response = cartDtoList
				.Select(cartDto => _mapper.Map<CartDto, CartResponse>(cartDto))
				.ToList();
			return Ok(response);
		}

		/// <summary>
		/// Retrieves the cart for a specific customer.
		/// </summary>
		/// <param name="customerId">The unique identifier of the customer.</param>
		/// <returns>
		/// The cart with its associated items for the specified customer.
		/// Returns <see cref="StatusCodes.Status404NotFound"/> if the cart does not exist.
		/// </returns>
		[HttpGet("/customer/{customerId}")]
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
	}
}
