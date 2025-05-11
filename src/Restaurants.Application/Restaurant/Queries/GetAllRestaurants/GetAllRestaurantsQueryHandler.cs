using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurant.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurant.Queries.GetAllRestaurants
{
	public class GetAllRestaurantsQueryHandler(IRestaurantRepository restaurantRepository,
        ILogger<GetAllRestaurantsQueryHandler> logger,
        IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
	{
		public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
		{
			logger.LogInformation("Getting all restaurants");

			var (restaurants, totalCount) = await restaurantRepository.GetAllMatchingAsync(
				request.SearchPhrase, 
				request.PageSize, 
				request.PageNumber,
				request.SortBy,
				request.SortDirecion);

			var restaurantDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

			var result = new PagedResult<RestaurantDto>(restaurantDto, totalCount, request.PageSize, request.PageNumber);

			return result;
		}
	}
}
