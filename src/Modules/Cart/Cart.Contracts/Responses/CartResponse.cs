using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Abysalto.Retail.Modules.Cart.Contracts.Responses
{
	/// <summary>
	/// Represents a customer's cart.
	/// <para>
	/// Includes cart details such as status, totals, timestamps, and the list of items.
	/// </para>
	/// </summary>
	public class CartResponse
	{
		/// <summary>
		/// The unique identifier of the cart.
		/// </summary>
		/// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
		[JsonPropertyName("id")]
		public Guid Id { get; set; }

		/// <summary>
		/// The unique identifier of the customer who owns the cart.
		/// </summary>
		/// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
		[JsonPropertyName("customerId")]
		public Guid CustomerId { get; set; }

		/// <summary>
		/// The current status of the cart.
		/// <para>Possible values: Active, Completed, Canceled, Abandoned.</para>
		/// </summary>
		/// <example>Active</example>
		[JsonPropertyName("status")]
		public string Status { get; set; } = string.Empty;

		/// <summary>
		/// The total monetary amount of all items in the cart.
		/// </summary>
		/// <example>10.50</example>
		[JsonPropertyName("totalAmount")]
		public decimal TotalAmount { get; set; }

		/// <summary>
		/// The total number of items in the cart.
		/// </summary>
		/// <example>3</example>
		[JsonPropertyName("totalItems")]
		public int TotalItems { get; set; }

		/// <summary>
		/// The list of items contained in the cart.
		/// </summary>
		[JsonPropertyName("items")]
		public List<CartItemResponse> Items { get; set; } = new();

		/// <summary>
		/// The date and time when the cart was created.
		/// </summary>
		/// <example>2026-10-01T00:00:00.000Z</example>
		[JsonPropertyName("createdAt")]
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// The date and time when the cart was last updated.
		/// </summary>
		/// <example>2026-10-01T12:00:00.000Z</example>
		[JsonPropertyName("updatedAt")]
		public DateTime? UpdatedAt { get; set; }
	}
}
