using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurant.Dtos;
using Restaurants.Application.Users;
using Restaurants.Domain.Interface;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurant.Commands.CreateRestaurant
{
	public class CreateRestaurantCommandHandler(IRestaurantRepository restaurantRepository,
        ILogger<CreateRestaurantCommandHandler> logger,
        IMapper mapper,
		IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, int>
	{

		public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
		{
			var currentUser = userContext.GetCurrentUser();

			logger.LogInformation("{UserName} {UserId} Creating a new restaurant: {@Restaurant}",
				currentUser.Email, 
				currentUser.Id, 
				request);

			var restaurant = mapper.Map<Domain.Entities.Restaurant>(request);
			restaurant.OwnerId = currentUser.Id;

			int id = await restaurantRepository.CreateAsync(restaurant);

			return id;
		}
	}
}
