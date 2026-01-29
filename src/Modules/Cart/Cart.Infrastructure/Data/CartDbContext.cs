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
			optionsBuilder
				.LogTo(Console.WriteLine, LogLevel.Debug) // Logs to Console
				.EnableSensitiveDataLogging();
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new CartConfiguration());
			modelBuilder.ApplyConfiguration(new CartItemConfiguration());

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