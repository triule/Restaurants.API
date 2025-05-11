using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurant.Commands.CreateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interface;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurant.Commands.UpdateRestaurant
{
	public class UpdateRestaurantCommandHandler(IRestaurantRepository restaurantRepository,
        ILogger<UpdateRestaurantCommandHandler> logger,
        IMapper mapper, 
		IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<UpdateRestaurantCommand>
	{
		
		public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
		{
			logger.LogInformation("Updating restaurant with id: {RestaurantId} with {@UpdatedRestaurant}", request.Id, request);
			var restaurant = await restaurantRepository.GetByIdAsync(request.Id);
			if (restaurant is null)
			{
				throw new NotFoundException(nameof(Domain.Entities.Restaurant), request.Id.ToString());
            }

            if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            {
                throw new ForbidException();
            }

            mapper.Map(request, restaurant);
			await restaurantRepository.SaveChanges();
		}
	}
}
