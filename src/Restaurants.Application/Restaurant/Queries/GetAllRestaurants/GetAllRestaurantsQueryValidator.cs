

using FluentValidation;
using Restaurants.Application.Restaurant.Dtos;

namespace Restaurants.Application.Restaurant.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
    {
        private int[] allowPageSize = [5, 10, 15, 30];
        private string[] allowedSortByColumnNames = [nameof(RestaurantDto.Name),
                    nameof(RestaurantDto.Category),
                    nameof(RestaurantDto.Description)];
        public GetAllRestaurantsQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize)
                .Must(value => allowPageSize.Contains(value))
                .WithMessage($"Page size must be in [{string.Join(", ", allowPageSize)}]");

            RuleFor(r => r.SortBy)
                .Must(value => allowedSortByColumnNames.Contains(value))
                .When(q => q.SortBy != null)
                .WithMessage($"Sort by is optional, or mus be in [{string.Join(", ", allowedSortByColumnNames)}]");
        }
    }
}
