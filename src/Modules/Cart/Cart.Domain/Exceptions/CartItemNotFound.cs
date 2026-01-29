using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abysalto.Retail.Modules.Cart.Domain.Entities;

namespace Abysalto.Retail.Modules.Cart.Domain.Exceptions
{
	public class CartItemNotFound : CartDomainException
	{
		private const string _message = "There is no such CartItem.";
		public CartItemNotFound() : base(_message) {}
		public CartItemNotFound(Exception inner) : base(_message, inner) { }
	}
}
