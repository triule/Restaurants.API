using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Restaurants.Application.Restaurant.Commands.CreateRestaurant;
using Restaurants.Application.Restaurant.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurant.Dtos
{
	public class RestaurantsProfile : Profile
	{
		public RestaurantsProfile()
		{
			CreateMap<Domain.Entities.Restaurant, RestaurantDto>()
				.ForMember(d => d.City, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.City))
				.ForMember(d => d.PostalCode, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
				.ForMember(d => d.Street, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
				.ForMember(d => d.Dishes, opt => opt.MapFrom(src => src.Dishes));

			CreateMap<CreateRestaurantCommand, Domain.Entities.Restaurant>()
				.ForMember(d => d.Address, opt => opt.MapFrom(
					src => new Address
					{
						City = src.City,
						PostalCode = src.PostalCode,
						Street = src.Street,
					}));
			CreateMap<UpdateRestaurantCommand, Domain.Entities.Restaurant>();
		}

	}
}
