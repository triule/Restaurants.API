using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Restaurants.Application.Common;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories
{
	public class RestaurantRepository : IRestaurantRepository
	{
		private readonly RestaurantDbContext dbContext;

		public RestaurantRepository(RestaurantDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<int> CreateAsync(Restaurant restaurant)
		{
			dbContext.Restaurants.Add(restaurant);
			await dbContext.SaveChangesAsync();
			return restaurant.Id;
		}

		public async Task<IEnumerable<Restaurant>> GetAllAsync()
		{
			var restaurants = await dbContext.Restaurants.ToListAsync();
			return restaurants;
		}

        public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(
			string? searchPhrase, 
			int pageSize, 
			int pageNumber, 
			string? sortBy,
			SortDirection sortDirecion)
        {
			var searchPhraseLower = searchPhrase?.ToLower();

			var baseQuery = dbContext.Restaurants.
				Where(o => searchPhrase == null || (o.Name.ToLower().Contains(searchPhraseLower)
												|| o.Description.ToLower().Contains(searchPhraseLower)));

			var totalCount = baseQuery.Count();

			if(sortBy != null)
			{
				var columnSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
				{
					{nameof(Restaurant.Name), r => r.Name },
					{nameof(Restaurant.Description), r => r.Description },
					{nameof(Restaurant.Category), r => r.Category },

				};

				var selectedColumn = columnSelector[sortBy];
					
				baseQuery = sortDirecion == SortDirection.Ascending
                    ? baseQuery.OrderBy(selectedColumn)
					: baseQuery.OrderByDescending(selectedColumn);
            }

            var restaurants = await baseQuery
                .Skip(pageSize * (pageNumber - 1))
				.Take(pageSize)
				.ToListAsync();
            return (restaurants, totalCount);
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
		{
			var restaurant = await dbContext.Restaurants
				.Include(r => r.Dishes)
				.FirstOrDefaultAsync(x => x.Id == id);
			return restaurant;
		}
		public Task SaveChanges() => dbContext.SaveChangesAsync();	

		public async Task DeleteAsync(Restaurant restaurant)
		{
			dbContext.Restaurants.Remove(restaurant);
			await dbContext.SaveChangesAsync();
		}

	}
}
