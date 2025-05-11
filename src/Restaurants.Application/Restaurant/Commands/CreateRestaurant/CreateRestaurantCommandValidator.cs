using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Restaurants.Application.Restaurant.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
    {
        private readonly string[] validCategories = ["Italian", "Indian", "Mexican", "Japanese", "American"];
        public CreateRestaurantCommandValidator()
        {
            RuleFor(x => x.Name)
                .Length(3, 100);    
            RuleFor(x => x.Category)
                .Must(validCategories.Contains)
                .WithMessage("Please provide category choosen fom valid categories");
            RuleFor(x => x.ContactEmail)
                .EmailAddress()
                .WithMessage("Please provide a valid email address");
            RuleFor(x => x.PostalCode)
                .Matches(@"^\d{2}-\d{3}")
                .WithMessage("Please provide a valid postal code (XX-XXX)");
        }
    }
}
