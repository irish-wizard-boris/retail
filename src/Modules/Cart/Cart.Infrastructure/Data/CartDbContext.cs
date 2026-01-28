using System.Collections.Generic;
using System.Reflection.Emit;
using Abysalto.Retail.Modules.Cart.Application.DTO;
using Abysalto.Retail.Modules.Cart.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Abysalto.Retail.Modules.Cart.Infrastructure.Data
{
	public class CartDbContext : DbContext
	{

		public DbSet<ShoppingCart> Carts { get; set; }
		public DbSet<ShoppingCartItem> CartItems { get; set; }

		public CartDbContext(DbContextOptions<CartDbContext> options) : base(options) {}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ShoppingCart>(builder =>
			{
				builder.ToTable("Carts");
				builder.HasKey(c => c.Id);

				builder.HasMany(c => c.Items)
					   .WithOne(i => i.Cart)
					   .HasForeignKey(i => i.CartId)
					   .OnDelete(DeleteBehavior.Cascade);

				builder.Ignore(c => c.TotalAmount);
				builder.Ignore(c => c.TotalItems);

				builder.Property(x => x.Id)
					.HasConversion(
						v => v.ToString().ToLower(),
						v => Guid.Parse(v)          
					);

				builder.Property(x => x.CustomerId)
					.HasConversion(
						v => v.ToString().ToLower(),
						v => Guid.Parse(v)          
					);

			});

			modelBuilder.Entity<ShoppingCartItem>(builder =>
			{
				builder.ToTable("CartItems");
				builder.HasKey(i => i.Id);

				builder.Ignore(i => i.TotalPrice);

				builder.Property(x => x.Id)
					.HasConversion(
						v => v.ToString().ToLower(),
						v => Guid.Parse(v)
					);

				builder.Property(x => x.CartId)
					.HasConversion(
						v => v.ToString().ToLower(),
						v => Guid.Parse(v)
					);

				builder.Property(x => x.ProductId)
					.HasConversion(
						v => v.ToString().ToLower(),
						v => Guid.Parse(v)
					);
			});
		}

		public override int SaveChanges()
		{
			UpdateTimestamps();
			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			UpdateTimestamps();
			return await base.SaveChangesAsync(cancellationToken);
		}

		private void UpdateTimestamps()
		{
			var entries = ChangeTracker.Entries<ShoppingCart>()
				.Where(e => e.State == EntityState.Modified);

			foreach (var entry in entries)
			{
				entry.Entity.UpdatedAt = DateTime.UtcNow;
			}

			var cartItemEntries = ChangeTracker.Entries<ShoppingCartItem>()
				.Where(e => e.State == EntityState.Added);

			foreach (var entry in cartItemEntries)
			{
				if (entry.Entity.AddedAt == default)
				{
					entry.Entity.AddedAt = DateTime.UtcNow;
				}
			}
		}

		public async Task<TResult> ExecuteInTransactionAsync<TResult>(
			Func<Task<TResult>> operation,
			CancellationToken cancellationToken = default)
		{
			var strategy = Database.CreateExecutionStrategy();

			return await strategy.ExecuteAsync(async () =>
			{
				using var transaction = await Database.BeginTransactionAsync(cancellationToken);
				try
				{
					var result = await operation();
					await transaction.CommitAsync(cancellationToken);
					return result;
				}
				catch
				{
					await transaction.RollbackAsync(cancellationToken);
					throw;
				}
			});
		}
	}
}