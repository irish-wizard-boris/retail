using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Abysalto.Retail.Modules.Cart.Contracts.Requests
{
	/// <summary>
	/// Represents a request to add an item to a customer's cart.
	/// <para>
	/// The request includes the product identifier and the quantity to add. 
	/// If the customer does not have a cart, one will be created.
	/// </para>
	/// </summary>
	public class AddItemToCartRequest
	{
		/// <summary>
		/// The unique identifier of the product to add to the cart.
		/// </summary>
		/// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
		[JsonPropertyName("productId")]
		[Required(ErrorMessage = "ProductId is required.")]
		public Guid? ProductId { get; set; }

		/// <summary>
		/// The number of units of the product to add.
		/// </summary>
		/// <example>3</example>
		[JsonPropertyName("quantity")]
		[Required(ErrorMessage = "Quantity is required.")]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
		public int? Quantity { get; set; }
	}
}
