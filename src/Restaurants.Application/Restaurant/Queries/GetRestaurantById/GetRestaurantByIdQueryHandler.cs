using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurant.Dtos;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interface;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurant.Queries.GetRestaurantById
{
	public class GetRestaurantByIdQueryHandler(IRestaurantRepository restaurantRepository,
        ILogger<GetRestaurantByIdQueryHandler> logger,
		IMapper mapper, 
		IBlobStorageService blobStorageService) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
	{
		public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
		{
			logger.LogInformation("Get restaurant by id {@RestaurantId}", request.Id);
			var restaurant = await restaurantRepository.GetByIdAsync(request.Id)
				?? throw new NotFoundException(nameof(Domain.Entities.Restaurant), request.Id.ToString());

			;
			var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);
            restaurantDto.LogoSasUrl =  blobStorageService.GetBlobSasUrl(restaurant.LogoUrl);


            return restaurantDto;
		}
	}
}
