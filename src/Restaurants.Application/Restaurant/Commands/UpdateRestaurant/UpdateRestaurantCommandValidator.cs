using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurants.Application.Restaurant.Commands.UpdateRestaurant
{
	public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
	{
        public UpdateRestaurantCommandValidator()
        {
            RuleFor(x => x.Name)
                .Length(3, 100);
        }
    }
}
