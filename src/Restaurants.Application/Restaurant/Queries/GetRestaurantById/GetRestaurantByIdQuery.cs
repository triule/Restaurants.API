using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Restaurants.Application.Restaurant.Dtos;

namespace Restaurants.Application.Restaurant.Queries.GetRestaurantById
{
	public class GetRestaurantByIdQuery : IRequest<RestaurantDto>
	{
        public GetRestaurantByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; }
	}
}
