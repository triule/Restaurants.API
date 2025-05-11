using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDish
{
    public class CreateDishCommandValidators : AbstractValidator<CreateDishCommand>
    {
        public CreateDishCommandValidators()
        {
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be a non-negative number");

            RuleFor(x => x.KiloCalories)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be a non-negative number");
        }
    }

}
