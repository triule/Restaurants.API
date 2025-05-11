using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Application.Users;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;
using static Restaurants.Infrastructure.Authorization.Constants;
using Restaurants.Infrastructure.Services;
using Restaurants.Domain.Interface;
using Restaurants.Infrastructure.Configuration;
using Restaurants.Infrastructure.Storage;

namespace Restaurants.Infrastructure.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("RestaurantsDb");
			service.AddDbContext<RestaurantDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
			});

			service.AddIdentityApiEndpoints<User>()
				.AddRoles<IdentityRole>()
				.AddClaimsPrincipalFactory<RestaurantUserClaimsPrincipalFactory>()
				.AddEntityFrameworkStores<RestaurantDbContext>();

			service.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
			service.AddScoped<IRestaurantRepository, RestaurantRepository>();
			service.AddScoped<IDishRepository, DishRepository>();
			service.AddScoped<IUserContext, UserContext>();
			service.AddHttpContextAccessor();
			service.AddAuthorizationBuilder()
				.AddPolicy(PolicyNames.HasNationality, builder => builder.RequireClaim(AppClaimTypes.Nationality, "hehe", "Vietnam"))
				.AddPolicy(PolicyNames.AtLeast20, builder => builder.AddRequirements(new MinimumAgeRequirement(20)))
				.AddPolicy(PolicyNames.CraetedAtLeast2Restaurants, builder => builder.AddRequirements(new CreateMultipleRestaurantsRequirement(2)));

            service.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            service.AddScoped<IAuthorizationHandler, CreateMultipleRestaurantsRequirementHandler>();
            service.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();

			service.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
			service.AddScoped<IBlobStorageService, BlobStorageService>();        
		}

    }
}
