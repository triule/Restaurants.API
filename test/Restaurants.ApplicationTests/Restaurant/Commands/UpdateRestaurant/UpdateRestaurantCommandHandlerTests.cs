using AutoMapper;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using RestaurantEntity = Restaurants.Domain.Entities.Restaurant;
using Restaurants.Domain.Interface;
using Restaurants.Domain.Repositories;
using Xunit;
using FluentAssertions;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurant.Commands.UpdateRestaurant.Tests
{
    public class UpdateRestaurantCommandHandlerTests
    {
        private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
        private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;
        private readonly UpdateRestaurantCommandHandler _handler;

        public UpdateRestaurantCommandHandlerTests()
        {
            _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
            _mapperMock = new Mock<IMapper>();
            _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

            _handler = new UpdateRestaurantCommandHandler(
                _restaurantRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object,
                _restaurantAuthorizationServiceMock.Object);
        }

        [Fact()]
        public async Task Handle_WithValidRequest_ShouldUpdateRestaurant()
        {
            // arrange
            var restaurantId = 1;
            var command = new UpdateRestaurantCommand
            {
                Id = restaurantId,
                Name = "New Test",
                Description = "New Description",
                HasDelivery = true,
            };

            var restaurant = new RestaurantEntity
            {
                Id = restaurantId,
                Name = "Test",
                Description = "Test"
            };

            _restaurantRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(restaurant);

            _restaurantAuthorizationServiceMock
                .Setup(m => m.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
                .Returns(true);

            // action
            await _handler.Handle(command, CancellationToken.None);

            // assert
            _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
            _restaurantRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
        {
            // Arrange
            var restaurantId = 2;
            var request = new UpdateRestaurantCommand
            {
                Id = restaurantId
            };

            _restaurantRepositoryMock
                .Setup(r => r.GetByIdAsync(restaurantId))
                .ReturnsAsync((RestaurantEntity?)null);

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Restaurant with id: {restaurantId} does not exist");
        }

        [Fact]
        public async Task Handle_WithoutPermission_ShouldThrowForbidException()
        {
            // Arrange
            var restaurantId = 3;
            var request = new UpdateRestaurantCommand
            {
                Id = restaurantId
            };
            var existingRestaurant = new RestaurantEntity
            {
                Id = restaurantId
            };

            _restaurantRepositoryMock
                .Setup(r => r.GetByIdAsync(restaurantId))
                .ReturnsAsync(existingRestaurant);

            _restaurantAuthorizationServiceMock
                .Setup(a => a.Authorize(existingRestaurant, ResourceOperation.Update))
                .Returns(false);

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ForbidException>();
        }


    }
}