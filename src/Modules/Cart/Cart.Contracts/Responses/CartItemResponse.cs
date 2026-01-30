using System;
using System.Text.Json.Serialization;

namespace Abysalto.Retail.Modules.Cart.Contracts.Responses
{
	/// <summary>
	/// Represents an item in a customer's cart.
	/// <para>
	/// Contains product details, quantity, pricing, and the date it was added to the cart.
	/// </para>
	/// </summary>
	public class CartItemResponse
	{
		/// <summary>
		/// The unique identifier of the cart item.
		/// </summary>
		/// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
		[JsonPropertyName("Id")]
		public Guid Id { get; set; }

		/// <summary>
		/// The unique identifier of the product.
		/// </summary>
		/// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
		[JsonPropertyName("productId")]
		public Guid ProductId { get; set; }

		/// <summary>
		/// The number of units of the product in the cart.
		/// </summary>
		/// <example>3</example>
		[JsonPropertyName("quantity")]
		public int Quantity { get; set; }

		/// <summary>
		/// The unit price of the product at the time it was added to the cart.
		/// </summary>
		/// <example>2.32</example>
		[JsonPropertyName("unitPrice")]
		public decimal UnitPrice { get; set; }

		/// <summary>
		/// The total price for this cart item (Quantity × UnitPrice).
		/// </summary>
		/// <example>6.96</example>
		[JsonPropertyName("totalPrice")]
		public decimal TotalPrice { get; set; }

		/// <summary>
		/// The date and time when the item was added to the cart.
		/// </summary>
		/// <example>2026-10-01T00:00:00.000Z</example>
		[JsonPropertyName("addedAt")]
		public DateTime AddedAt { get; set; }
	}
}
