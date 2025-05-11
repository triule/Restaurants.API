using Xunit;
using Restaurants.Application.Restaurant.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Restaurants.Domain.Entities;
using FluentAssertions;
using Restaurants.Application.Restaurant.Commands.CreateRestaurant;
using Restaurants.Application.Restaurant.Commands.UpdateRestaurant;

namespace Restaurants.Application.Restaurant.Dtos.Tests
{
    public class RestaurantsProfileTests
    {
        private readonly IMapper _mapper;

        public RestaurantsProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RestaurantsProfile>();
            });
            _mapper = configuration.CreateMapper();
        }
        [Fact()]
        public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
        {
            // arrange

            var restaurant = new Restaurants.Domain.Entities.Restaurant
            {
                Id = 1,
                Name = "Test restaurant",
                Description = "Test description",
                Category = "Test category",
                HasDelivery = true,
                ContactEmail = "test@example.com",
                ContactNumber = "123456789",
                Address = new Address
                {
                    City = "Test city",
                    Street = "Test street",
                    PostalCode = "12345"
                }
            };
            // action
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            // assert
            restaurant.Should().NotBeNull();
            restaurantDto.Id.Should().Be(restaurant.Id);
            restaurantDto.Name.Should().Be(restaurant.Name);
            restaurantDto.Description.Should().Be(restaurant.Description);
            restaurantDto.Category.Should().Be(restaurant.Category);
            restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
            restaurantDto.City.Should().Be(restaurant.Address.City);
            restaurantDto.Street.Should().Be(restaurant.Address.Street);
            restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);

        }

        [Fact()]
        public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
        {
            // arrange
            var command = new CreateRestaurantCommand
            {
                Name = "Test restaurant",
                Description = "Test description",
                Category = "Test category",
                HasDelivery = true,
                ContactEmail = "test@example.com",
                ContactNumber = "123456789",
                City = "Test city",
                Street = "Test street",
                PostalCode = "12345",
            };
            // action
            var restaurant = _mapper.Map<Restaurants.Domain.Entities.Restaurant>(command);

            // assert
            restaurant.Should().NotBeNull();
            restaurant.Name.Should().Be(command.Name);
            restaurant.Description.Should().Be(command.Description);
            restaurant.Category.Should().Be(command.Category);
            restaurant.HasDelivery.Should().Be(command.HasDelivery);
            restaurant.ContactEmail.Should().Be(command.ContactEmail);
            restaurant.ContactNumber.Should().Be(command.ContactNumber);
            restaurant.Address.Should().NotBeNull();
            restaurant.Address.City.Should().Be(command.City);
            restaurant.Address.Street.Should().Be(command.Street);
            restaurant.Address.PostalCode.Should().Be(command.PostalCode);
        }

        [Fact()]
        public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
        {
            // arrange
            var command = new UpdateRestaurantCommand
            {
                Id = 1,
                Name = "Test restaurant",
                Description = "Test description",
                HasDelivery = true,
            };
            // action
            var restaurant = _mapper.Map<Restaurants.Domain.Entities.Restaurant>(command);

            // assert
            restaurant.Should().NotBeNull();
            restaurant.Id.Should().Be(command.Id);
            restaurant.Name.Should().Be(command.Name);
            restaurant.Description.Should().Be(command.Description);
            restaurant.HasDelivery.Should().Be(command.HasDelivery);


        }
    }
}