using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Restaurants.Application.Restaurant.Commands.DeleteRestaurant
{
	public class DeleteRestaurantCommand : IRequest
	{
		public DeleteRestaurantCommand(int id)
		{
			Id = id;
		}
		public int Id { get; set; }
	}
}
