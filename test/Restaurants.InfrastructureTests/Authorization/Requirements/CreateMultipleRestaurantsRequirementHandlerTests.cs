using Xunit;
using RestaurantEntity = Restaurants.Domain.Entities.Restaurant;
using Restaurants.Application.Users;
using Moq;
using Restaurants.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using FluentAssertions;


namespace Restaurants.Infrastructure.Authorization.Requirements.Tests
{
    public class CreateMultipleRestaurantsRequirementHandlerTests
    {
        [Fact()]
        public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@gmail.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m => m.GetCurrentUser())
                .Returns(currentUser);

            var restaurants = new List<RestaurantEntity>()
            {
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = "2"
                }
            };

            var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            restaurantRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(restaurants);

            var requirement = new CreateMultipleRestaurantsRequirement(2);
            var handler = new CreateMultipleRestaurantsRequirementHandler(restaurantRepositoryMock.Object, 
                userContextMock.Object);
            var context = new AuthorizationHandlerContext([requirement], null, null);

            // act
            await handler.HandleAsync(context);

            // assert
            context.HasSucceeded.Should().BeTrue();
        }

        [Fact()]
        public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@gmail.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m => m.GetCurrentUser())
                .Returns(currentUser);

            var restaurants = new List<RestaurantEntity>()
            {
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = "2"
                }
            };

            var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            restaurantRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(restaurants);

            var requirement = new CreateMultipleRestaurantsRequirement(2);
            var handler = new CreateMultipleRestaurantsRequirementHandler(restaurantRepositoryMock.Object,
                userContextMock.Object);
            var context = new AuthorizationHandlerContext([requirement], null, null);

            // act
            await handler.HandleAsync(context);

            // assert
            context.HasSucceeded.Should().BeFalse();
            context.HasFailed.Should().BeTrue();

        }
    }
}