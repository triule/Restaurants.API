using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
	public interface IRestaurantRepository
	{
		Task<IEnumerable<Restaurant>> GetAllAsync();
		Task<Restaurant?> GetByIdAsync(int id);
		Task<int> CreateAsync(Restaurant restaurant);
		Task DeleteAsync(Restaurant restaurant);
		Task SaveChanges();
		Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhraseLower, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
    }
}
