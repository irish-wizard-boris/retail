using Abysalto.Retail.Modules.Cart.Application.Services;
using Abysalto.Retail.Modules.Cart.Contracts.Requests.Validation;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Abysalto.Retail.Modules.Cart.Application.Extensions
{
	public static class CartApplicationExtensions
	{
		public static IServiceCollection AddCartApplication(
			this IServiceCollection services)
		{
			AddAutoMapperConfiguration(services);

			services.AddScoped<ICartService, CartService>();

			return services;
		}

		private static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
		{
			services.AddAutoMapper(cfg =>
			{
				cfg.AddProfile<CartMappingProfile>();
				cfg.AllowNullCollections = true;
				cfg.AllowNullDestinationValues = true;
			});

			return services;
		}

		private static IServiceCollection AddFluentValidation(this IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<AddItemToCartRequestValidator>();

			return services;
		}
	}
}
