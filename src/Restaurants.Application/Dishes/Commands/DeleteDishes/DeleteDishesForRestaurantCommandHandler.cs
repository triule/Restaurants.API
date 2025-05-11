using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes
{
    public class DeleteDishesForRestaurantCommandHandler(
        ILogger<DeleteDishesForRestaurantCommandHandler> logger,
        IRestaurantRepository restaurantRepository,
        IDishRepository dishRepository) : IRequestHandler<DeleteDishesForRestaurantCommand>
    {
        public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogWarning("Removing all dishes from {RestaurantId}", request.RestaurantId);

            var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Restaurant), request.RestaurantId.ToString());
            }
            await dishRepository.DeleteAsync(restaurant.Dishes);
            
        }
    }
}
