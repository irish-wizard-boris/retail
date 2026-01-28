using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abysalto.Retail.Modules.Cart.Application.DTO;
using Abysalto.Retail.Modules.Cart.Contracts.Requests;
using Abysalto.Retail.Modules.Cart.Contracts.Responses;
using Abysalto.Retail.Modules.Cart.Domain.Entities;
using Abysalto.Retail.Modules.Cart.Domain.Enums;
using AutoMapper;

namespace Abysalto.Retail.Modules.Cart.Application
{
	public class CartMappingProfile : Profile
	{
		public CartMappingProfile()
		{
			// Domain to DTOs with Reverse
			CreateMap<ShoppingCart, CartDto>()
			.ForMember(dest => dest.Status,
				opt => opt.MapFrom(src => src.Status.ToString()))
			.ForMember(dest => dest.Items,
				opt => opt.MapFrom(src => src.Items))
			.ReverseMap()
			.ForMember(dest => dest.Status,
				opt => opt.MapFrom(src => Enum.Parse<CartStatus>(src.Status, true)));

			CreateMap<ShoppingCartItem, CartItemDto>()
			.ForMember(dest => dest.Cart,
				opt => opt.Ignore());

			CreateMap<CartItemDto, ShoppingCartItem>()
				.ForMember(dest => dest.Cart,
					opt => opt.Ignore());


			// Requests to DTO
			CreateMap<AddItemToCartRequest, CartItemDto>()
			.ForMember(dest => dest.Id,
				opt => opt.Ignore()) // generated later
			.ForMember(dest => dest.CartId,
				opt => opt.Ignore()) // set by application service
			.ForMember(dest => dest.Cart,
				opt => opt.Ignore()) // aggregate root controls this
			.ForMember(dest => dest.AddedAt,
				opt => opt.Ignore())
			.ForMember(dest => dest.TotalPrice,
				opt => opt.Ignore());


			// DTOs to requests
			CreateMap<CartDto, CartResponse>()
				.ForMember(dest => dest.Id,
					opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.CustomerId,
					opt => opt.MapFrom(src => src.CustomerId))
				.ForMember(dest => dest.Status,
					opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.TotalAmount,
					opt => opt.MapFrom(src => src.TotalAmount))
				.ForMember(dest => dest.TotalItems,
					opt => opt.MapFrom(src => src.TotalItems))
				.ForMember(dest => dest.Items,
					opt => opt.MapFrom(src => src.Items))
				.ForMember(dest => dest.CreatedAt,
					opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.UpdatedAt,
					opt => opt.MapFrom(src => src.UpdatedAt));

			CreateMap<CartItemDto, CartItemResponse>()
				.ForMember(dest => dest.Id,
					opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.ProductId,
					opt => opt.MapFrom(src => src.ProductId))
				.ForMember(dest => dest.Quantity,
					opt => opt.MapFrom(src => src.Quantity))
				.ForMember(dest => dest.UnitPrice,
					opt => opt.MapFrom(src => src.UnitPrice))
				.ForMember(dest => dest.TotalPrice,
					opt => opt.MapFrom(src => src.TotalPrice))
				.ForMember(dest => dest.AddedAt,
					opt => opt.MapFrom(src => src.AddedAt));

		}
	}
}
