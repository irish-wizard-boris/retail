using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abysalto.Retail.Modules.Cart.Domain.Entities;
using Abysalto.Retail.Modules.Cart.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abysalto.Retail.Modules.Cart.Infrastructure.Data
{
	public class CartConfiguration : IEntityTypeConfiguration<ShoppingCart>
	{
		public void Configure(EntityTypeBuilder<ShoppingCart> builder)
		{
			builder.ToTable("Carts", "cart");

			builder.HasKey(c => c.Id);
			builder.Property(c => c.Id)
				.HasColumnName("Id")
				.ValueGeneratedNever()
				.HasConversion(
					v => v.ToString().ToLower(),
					v => Guid.Parse(v)
				); 

			builder.HasIndex(c => c.CustomerId);
			builder.Property(c => c.CustomerId)
				.HasColumnName("CustomerId")
				.IsRequired()
				.HasConversion(
					v => v.ToString().ToLower(),
					v => Guid.Parse(v)
				);

			builder.Property(c => c.Status)
				.HasColumnName("Status")
				.HasConversion(
					v => v.ToString(),
					v => (CartStatus)Enum.Parse(typeof(CartStatus), v))
				.HasMaxLength(20)
				.IsRequired();

			builder.Property(c => c.CreatedAt)
				.HasColumnName("CreatedAt")
				.IsRequired()
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.Property(c => c.UpdatedAt)
				.HasColumnName("UpdatedAt")
				.IsRequired(false);

			builder.HasMany(c => c.Items)
				.WithOne(i => i.Cart)
				.HasForeignKey(i => i.CartId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Ignore(c => c.DomainEvents);

			builder.Ignore(c => c.TotalItems);
			builder.Ignore(c => c.TotalAmount);

			builder.ToTable(t => t.HasCheckConstraint("CK_Cart_Status",
				"Status IN ('Active', 'Abandoned', 'Converted')"));
		}
	}
}
