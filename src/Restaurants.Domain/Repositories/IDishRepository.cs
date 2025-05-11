using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IDishRepository
    {
        Task<IEnumerable<Dish>> GetAllAsync();
        Task<Dish?> GetByIdAsync(int id);
        Task<int> CreateAsync(Dish dish);
        Task DeleteAsync(IEnumerable<Dish> entities);
        Task SaveChanges();
    }
}
