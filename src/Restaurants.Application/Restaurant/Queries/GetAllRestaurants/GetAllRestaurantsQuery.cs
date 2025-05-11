using System;

using MediatR;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurant.Dtos;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurant.Queries.GetAllRestaurants
{
	public class GetAllRestaurantsQuery : IRequest<PagedResult<RestaurantDto>>
	{
        public string? SearchPhrase { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SortBy { get; set; }
        public SortDirection SortDirecion { get; set; }

    }
}
