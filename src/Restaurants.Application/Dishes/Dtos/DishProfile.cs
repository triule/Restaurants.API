using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.Dtos
{
	public class DishProfile: Profile
	{
        public DishProfile()
        {
            CreateMap<Dish, DishDto>();
            CreateMap<CreateDishCommand, Dish>();
        }
    }
}
