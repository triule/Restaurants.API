using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interface;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurant.Commands.UploadRestaurantLogo
{
    public class UploadRestaurantLogoCommandHandler(
        ILogger<UploadRestaurantLogoCommandHandler> logger,
        IRestaurantRepository restaurantRepository,
        IMapper mapper,
        IRestaurantAuthorizationService restaurantAuthorizationService,
        IBlobStorageService blobStorageService) : IRequestHandler<UploadRestaurantLogoCommand>
    {
        async Task IRequestHandler<UploadRestaurantLogoCommand>.Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Uploading restaurant logo for id: {RestaurantId}", request.RestaurantId);
            var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
            }
            if(!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            {
                throw new ForbidException();
            }

            var logoUrl = await blobStorageService.UploadToBlobAsync(request.File, request.FileName);
            restaurant.LogoUrl  = logoUrl;
            await restaurantRepository.SaveChanges();
        }
    }
}
