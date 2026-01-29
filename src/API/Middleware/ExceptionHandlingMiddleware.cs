using Abysalto.Retail.Modules.Cart.Domain.Entities;

namespace Abysalto.Retail.API.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (CartDomainException ex)
			{
				context.Response.StatusCode = StatusCodes.Status400BadRequest;
				await context.Response.WriteAsJsonAsync(new
				{
					error = "Domain Error.",
					message = ex.Message
				});
			}
			catch (Exception ex)
			{
				// Fallback for real 500 errors
				context.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await context.Response.WriteAsJsonAsync(new { error = "Internal Server Error" });
			}
		}
	}
}
