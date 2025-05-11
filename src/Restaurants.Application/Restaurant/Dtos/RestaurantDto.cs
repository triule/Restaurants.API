using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurant.Dtos
{
	public class RestaurantDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = default!;
		public string Description { get; set; } = default!;
		public string Category { get; set; } = default!;
		public bool HasDelivery { get; set; }
		public string? City { get; set; }
		public string? Street { get; set; }
		public string? PostalCode { get; set; }
		public string? LogoSasUrl { get; set; }
		public List<DishDto> Dishes { get; set; } = [];

	}
}
