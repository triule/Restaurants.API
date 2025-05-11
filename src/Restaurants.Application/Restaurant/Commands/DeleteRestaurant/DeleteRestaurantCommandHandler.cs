using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurant.Commands.CreateRestaurant;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interface;
using Restaurants.Domain.Constants;


namespace Restaurants.Application.Restaurant.Commands.DeleteRestaurant
{
	public class DeleteRestaurantCommandHandler(IRestaurantRepository restaurantRepository,
        ILogger<CreateRestaurantCommandHandler> logger,
        IMapper mapper, 
		IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
	{
		public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
		{
			logger.LogInformation("Delete restaurant with id: {@RestaurantId}", request.Id);
			var restaurant = await restaurantRepository.GetByIdAsync(request.Id);
			if (restaurant == null)
			{
				throw new NotFoundException(nameof(Domain.Entities.Restaurant), request.Id.ToString());
			}

			if(!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
			{
				throw new ForbidException();
			}
			await restaurantRepository.DeleteAsync(restaurant);
		}
	}
}
