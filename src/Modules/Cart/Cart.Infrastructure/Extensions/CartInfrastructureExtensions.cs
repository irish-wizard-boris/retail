using Abysalto.Retail.Modules.Cart.Domain.Repositories;
using Abysalto.Retail.Modules.Cart.Infrastructure.Data;
using AbysaltoRetail.Modules.Cart.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Abysalto.Retail.Modules.Cart.Infrastructure.Extensions
{
    public static class CartInfrastructureExtensions
    {
		public static IServiceCollection AddCartInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<ICartRepository, CartRepository>();

			//ar dbPath = Path.Combine(services.Environment.ContentRootPath, "db", "app.db");

			//var dbPath = Path.Combine(AppContext.BaseDirectory, "cart.db");
			
			//var connectionString =
			//	configuration.GetConnectionString("CartDb");

			//services.AddDbContext<CartDbContext>(options =>
			//{
			//	options.UseSqlite($"Data Source={dbPath}")
			//		   .EnableSensitiveDataLogging()
			//		   .EnableDetailedErrors()
			//		   .LogTo(Console.WriteLine); // logs SQL to console
			//});

			return services;
		}
	}
}
