using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;

namespace Abysalto.Retail.API.Filters
{


	public class ValidationFilter : IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			// 1. Find all arguments passed to the controller (e.g., AddItemToCartRequest)
			foreach (var argument in context.ActionArguments.Values)
			{
				if (argument == null) continue;

				// 2. See if there is a validator registered for this type
				var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
				var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

				if (validator != null)
				{
					// 3. Run validation
					var validationContext = new ValidationContext<object>(argument);
					var result = await validator.ValidateAsync(validationContext);

					if (!result.IsValid)
					{
						// 4. If invalid, stop the request and return 400
						context.Result = new BadRequestObjectResult(result.ToDictionary());
						return;
					}
				}
			}

			await next();
		}
	}
}
