using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements
{
    internal class CreateMultipleRestaurantsRequirementHandler(IRestaurantRepository restaurantRepository,
        IUserContext userContext) : AuthorizationHandler<CreateMultipleRestaurantsRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateMultipleRestaurantsRequirement requirement)
        {
            var currentUser = userContext.GetCurrentUser();

            var restaurant = await restaurantRepository.GetAllAsync();

            var userRestaurantCreated = restaurant.Count(o => o.OwnerId == currentUser!.Id);
            if (userRestaurantCreated >= requirement.MinimumRestaurantsCreated)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
