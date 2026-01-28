using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Abysalto.Retail.Modules.Cart.Contracts.Requests.Validation
{
    public class AddItemToCartRequestValidator : AbstractValidator<AddItemToCartRequest>
	{
		public AddItemToCartRequestValidator()
		{
			RuleFor(x => x.ProductId)
				.NotNull().WithMessage("Product ID is missing from the request")
				.NotEmpty().WithMessage("Product ID cannot be an empty GUID (all zeros)");

			RuleFor(x => x.Quantity)
				.NotNull().WithMessage("Quantity is required")
				.GreaterThan(0).WithMessage("Quantity must be at least 1");
		}
    }
}
