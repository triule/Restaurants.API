using Xunit;

using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Repositories;
using AutoMapper;
using RestaurantEntity = Restaurants.Domain.Entities.Restaurant;
using Restaurants.Application.Users;
using FluentAssertions;


namespace Restaurants.Application.Restaurant.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
        {
            // arrange
            var command = new CreateRestaurantCommand();
            var restaurant = new RestaurantEntity();

            var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();

            var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            restaurantRepositoryMock
                .Setup(repo => repo.CreateAsync(It.IsAny<RestaurantEntity>()))
                .ReturnsAsync(1);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<RestaurantEntity>(command))
                .Returns(restaurant);

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("owner-id", "test@test.com", [], null, null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var commandHandler = new CreateRestaurantCommandHandler(
                restaurantRepositoryMock.Object, 
                loggerMock.Object, 
                mapperMock.Object, 
                userContextMock.Object);

            // act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // assert
            result.Should().Be(1);
            restaurant.OwnerId.Should().Be("owner-id");
        }
    }
}