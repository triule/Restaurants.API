using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using CleanArchitecture_AzureTests;
using Moq;
using Restaurants.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Domain.Entities;
using System.Net.Http.Json;
using Restaurants.Application.Restaurant.Dtos;
using Restaurants.Infrastructure.Seeders;

namespace CleanArchitecture_Azure.Controllers.Tests
{
    public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock = new();
        private readonly Mock<IRestaurantSeeder> _restaurantsSeederMock = new();
        private readonly HttpClient _client;

        public RestaurantControllerTests(WebApplicationFactory<Program> factory)
        {
            _applicationFactory = factory.WithWebHostBuilder(builder => {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantRepository), _ => _restaurantRepositoryMock.Object));

                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantSeeder), _ => _restaurantsSeederMock.Object));

                });
            });
        }

        [Fact()]
        public async Task GetbyId_ForNonExistingId_ShouldReturn404NotFound()
        {
            // arrange
            var id = 1123;
            _restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);
            var client = _applicationFactory.CreateClient();

            // act
            var result = await client.GetAsync($"/api/restaurants/{id}");

            // assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact()]
        public async Task GetbyId_ForExistingId_ShouldReturn200Ok()
        {
            // arrange
            var id = 1;
            var restaurant = new Restaurant()
            {
                Id = id,
                Name = "Test",
                Description = "Test desc"
            };

            _restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);
            var client = _applicationFactory.CreateClient();

            // act
            var response = await client.GetAsync($"/api/restaurants/{id}");
            var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            restaurantDto.Should().NotBeNull();
            restaurantDto.Name.Should().Be("Test");
            restaurant.Description.Should().Be("Test desc");
        }

        [Fact()]
        public async Task GetAll_ForValidRequest_Returns200Ok()
        {
            // arrange
            var client= _applicationFactory.CreateClient();

            // act
            var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

            // assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact()]
        public async Task GetAll_ForInValidRequest_Returns400BadRequest()
        {
            // arrange
            var client = _applicationFactory.CreateClient();

            // act
            var result = await client.GetAsync("/api/restaurants");

            // assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}