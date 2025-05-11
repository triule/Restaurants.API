using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interface;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.Infrastructure.Services
{
    public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
        IUserContext userContext) : IRestaurantAuthorizationService
    {
        public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
        {
            var user = userContext.GetCurrentUser();

            logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
                user.Email,
                resourceOperation,
                restaurant.Name);

            if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
            {
                logger.LogInformation("Creare/Read operation - successful authorization");
                return true;
            }

            if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
            {
                logger.LogInformation("Admin user, delete operation - successful authorization");
                return true;
            }
            if (resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update
                && user.Id == restaurant.OwnerId)
            {
                logger.LogInformation("Restaurant Owner - Successful authorization");
                return true;
            }
            return false;
        }
    }
}
