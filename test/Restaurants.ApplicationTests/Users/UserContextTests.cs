﻿using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            // arange
            var dateOfBirth = new DateOnly(1999, 1, 1);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier, "1"),
                new (ClaimTypes.Email, "test@gmail.com"),
                new (ClaimTypes.Role, UserRoles.Admin),
                new (ClaimTypes.Role, UserRoles.User),
                new (ClaimTypes.NameIdentifier, "1"),
                new ("Nationality", "German"),
                new ("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthType"));

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            });
            var userContext = new UserContext(httpContextAccessorMock.Object);

            // act
            var currentUser = userContext.GetCurrentUser();


            // assert
            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@gmail.com");
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
            currentUser.Nationality.Should().Be("German");
            currentUser.DateOfBirth.Should().Be(dateOfBirth);


        }

        public void GetCurrentUser_WithUserContextNotPresent_ThrowInvalidOperationException()
        {
            // arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

            var userContext = new UserContext(httpContextAccessorMock.Object);

            // act
            Action action = () => userContext.GetCurrentUser();

            // assert
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("User context is not present");
        }
    }
}