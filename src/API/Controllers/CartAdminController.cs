using Abysalto.Retail.Modules.Cart.Application.DTO;
using Abysalto.Retail.Modules.Cart.Application.Services;
using Abysalto.Retail.Modules.Cart.Contracts.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Abysalto.Retail.API.Controllers
{
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

		[HttpGet]
		public async Task<IActionResult> GetAllCartsAsync()
		{
			var cartDtoList = await _cartService.GetAllCartsAsync();
			var response = cartDtoList.Select(cartDto => _mapper.Map<CartDto, CartResponse>(cartDto)).ToList();
			return Ok(response);
		}

		[HttpGet("/customer/{customerId}")]
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
