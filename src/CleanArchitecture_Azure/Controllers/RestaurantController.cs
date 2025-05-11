using Restaurants.Application.Restaurant;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurant.Dtos;
using Newtonsoft.Json;
using MediatR;
using Restaurants.Application.Restaurant.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurant.Queries.GetRestaurantById;
using Restaurants.Application.Restaurant.Commands.CreateRestaurant;
using Restaurants.Application.Restaurant.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurant.Commands.UpdateRestaurant;
using Microsoft.AspNetCore.Authorization;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;
using static Restaurants.Infrastructure.Authorization.Constants;
using System.Security.Claims;
using Restaurants.Application.Restaurant.Commands.UploadRestaurantLogo;

namespace CleanArchitecture_Azure.Controllers
{
	[ApiController]
	[Route("api/restaurants")]
    [Authorize]
    public class RestaurantController : ControllerBase
	{
		private readonly IMediator mediator;

		public RestaurantController(IMediator mediator)
		{
			this.mediator = mediator;
		}
		[HttpGet]
        [AllowAnonymous]
        //[Authorize(Policy = PolicyNames.CraetedAtLeast2Restaurants)]

        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery? query)
		{
            //var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var restaurants = await mediator.Send(query);
			return Ok(restaurants);
		}

		[HttpGet("{id}")]
		//[Authorize(Policy = PolicyNames.HasNationality)]
		public async Task<ActionResult<RestaurantDto?>> GetById([FromRoute] int id)
		{
			var restaurants = await mediator.Send(new GetRestaurantByIdQuery(id));
			return Ok(restaurants);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
		{
			await mediator.Send(new DeleteRestaurantCommand(id));			
			return NoContent();
		}

		[HttpPost]
		[Authorize(Roles =UserRoles.Owner)]
		public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand command)
		{
			if (command == null) return BadRequest("Payload không hợp lệ");
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			int id = await mediator.Send(command);
			return CreatedAtAction(nameof(GetById), new { id }, null);
		}
		[HttpPatch("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, UpdateRestaurantCommand command)
		{
			command.Id = id;
			await mediator.Send(command);
			return NoContent();
		}

        [HttpPost("{id}/logo")]
        public async Task<IActionResult> UploadLogo([FromRoute]int id, IFormFile file)
        {
			using var stream = file.OpenReadStream();
			var command = new UploadRestaurantLogoCommand()
			{
				RestaurantId = id,
				FileName = $"{id}-{file.FileName}",
				File = stream
            };
			await mediator.Send(command);
			return NoContent();
        }
    }
}
