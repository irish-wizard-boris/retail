using Abysalto.Retail.Modules.Cart.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Abysalto.Retail.Modules.Cart.Infrastructure.Data
{
	public class CartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
	{
		public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
		{
			builder.ToTable("CartItems", "cart");

			builder.HasKey(i => i.Id);
			builder.Property(i => i.Id)
				.HasColumnName("Id")
				.ValueGeneratedNever(); 

			builder.HasIndex(i => i.CartId);
			builder.Property(i => i.CartId)
				.HasColumnName("CartId")
				.IsRequired();

			builder.HasIndex(i => i.ProductId);
			builder.Property(i => i.ProductId)
				.HasColumnName("ProductId")
				.IsRequired();

			builder.Property(i => i.Quantity)
				.HasColumnName("Quantity")
				.IsRequired()
				.HasDefaultValue(1);

			builder.Property(i => i.UnitPrice)
				.HasColumnName("UnitPrice")
				.HasColumnType("decimal(10,2)")
				.IsRequired();

			builder.Property(i => i.AddedAt)
				.HasColumnName("AddedAt")
				.IsRequired()
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.HasIndex(i => new { i.CartId, i.ProductId })
				.IsUnique();

			builder.Ignore(i => i.DomainEvents);

			builder.Ignore(i => i.TotalPrice);

			builder.ToTable(t => t.HasCheckConstraint("CK_CartItem_Quantity",
				"Quantity > 0"));

			builder.ToTable(t => t.HasCheckConstraint("CK_CartItem_Price",
				"Price > 0"));
		}
	}
}