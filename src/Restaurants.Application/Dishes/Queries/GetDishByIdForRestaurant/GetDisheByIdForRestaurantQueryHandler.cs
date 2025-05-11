using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant
{
    public class GetDisheByIdForRestaurantQueryHandler(
        ILogger<GetDisheByIdForRestaurantQueryHandler> logger, 
        IRestaurantRepository restaurantRepository, 
        IMapper mapper) : IRequestHandler<GetDisheByIdForRestaurantQuery, DishDto>
    {
        public async Task<DishDto> Handle(GetDisheByIdForRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving dishes: {DishId} for restaurant with Id: {RestaurantId}",
                request.DishId, 
                request.RestaurantId);

            var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);

            if (restaurant == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Restaurant), request.RestaurantId.ToString());
            }
            var dish  = restaurant.Dishes.FirstOrDefault(x => x.Id == request.DishId);

            if (dish == null)
            {
                throw new NotFoundException(nameof(Dish), request.DishId.ToString());
            }
            var result = mapper.Map<DishDto>(dish);

            return result;    
        }
    }
}
