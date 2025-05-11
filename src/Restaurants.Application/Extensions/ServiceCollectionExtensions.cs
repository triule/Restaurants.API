using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurant;

namespace Restaurants.Application.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddApplication(this IServiceCollection services)
		{
			var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
			services.AddAutoMapper(applicationAssembly);

			services.AddValidatorsFromAssembly(applicationAssembly)
				.AddFluentValidationAutoValidation();
		}
	}
}
