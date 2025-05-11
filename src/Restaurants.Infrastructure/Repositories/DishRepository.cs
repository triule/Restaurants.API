using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories
{
    class DishRepository(RestaurantDbContext dbContext) : IDishRepository
    {

        public async Task<int> CreateAsync(Dish dish)
        {
            dbContext.Dishes.Add(dish);
            await dbContext.SaveChangesAsync();
            return dish.Id;
        }

        public async Task DeleteAsync(IEnumerable<Dish> dishes)
        {
            dbContext.Dishes.RemoveRange(dishes);
            await dbContext.SaveChangesAsync();
        }

        public Task<IEnumerable<Dish>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dish?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
