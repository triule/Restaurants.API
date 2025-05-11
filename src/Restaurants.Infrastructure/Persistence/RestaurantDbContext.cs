using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Restaurants.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Restaurants.Infrastructure.Persistence
{
	public class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : IdentityDbContext<User>(options)
	{
        internal DbSet<Restaurant> Restaurants { get; set; }
		internal DbSet<Dish> Dishes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Restaurant>()
				.OwnsOne(r => r.Address);

			modelBuilder.Entity<Restaurant>()
				.HasMany(r => r.Dishes).WithOne().HasForeignKey(r => r.RestaurantId);

			modelBuilder.Entity<User>()
				.HasMany(o => o.OwnedRestaurants)
				.WithOne(r => r.Owner)
				.HasForeignKey(r => r.OwnerId);
		}
	}
}
