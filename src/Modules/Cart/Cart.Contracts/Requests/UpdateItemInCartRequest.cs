using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Abysalto.Retail.Modules.Cart.Contracts.Requests
{
	/// <summary>
	/// Represents a request to update an item in a customer's cart.
	/// <para>
	/// The request includes the product identifier and the updated quantity. 
	/// The product must already exist in the customer's cart.
	/// </para>
	/// </summary>
	public class UpdateItemInCartRequest
	{
		/// <summary>
		/// The unique identifier of the product to update in the cart.
		/// </summary>
		/// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
		[JsonPropertyName("productId")]
		[Required(ErrorMessage = "ProductId is required.")]
		public Guid ProductId { get; set; }

		/// <summary>
		/// The new quantity of the product in the cart.
		/// </summary>
		/// <example>3</example>
		[JsonPropertyName("quantity")]
		[Required(ErrorMessage = "Quantity is required.")]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
		public int Quantity { get; set; }
	}
}
