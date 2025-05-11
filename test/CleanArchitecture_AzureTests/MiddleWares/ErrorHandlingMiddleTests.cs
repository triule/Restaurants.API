using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using Restaurants.Domain.Exceptions;

namespace CleanArchitecture_Azure.MiddleWares.Tests
{
    public class ErrorHandlingMiddleTests
    {
        [Fact()]
        public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
        {
            // arrange

            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);
            var context = new DefaultHttpContext();
            var nextDelegateMock = new Mock<RequestDelegate>();

            // act
            await middleware.InvokeAsync(context, nextDelegateMock.Object);

            // assert

            nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
        }

        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
        {
            // arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);
            var context = new DefaultHttpContext();
            var notFoundException = new NotFoundException(nameof(Restaurants.Domain.Entities.Restaurant), "1");

            // act
            await middleware.InvokeAsync(context, _ => throw notFoundException);

            // assert
            context.Response.StatusCode.Should().Be(404);
        }

        [Fact()]
        public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCode403()
        {
            // arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);
            var context = new DefaultHttpContext();
            var exception = new ForbidException();

            // act
            await middleware.InvokeAsync(context, _ => throw exception);

            // assert
            context.Response.StatusCode.Should().Be(403);
        }

        [Fact()]
        public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCode500()
        {
            // arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);
            var context = new DefaultHttpContext();
            var exception = new Exception();

            // act
            await middleware.InvokeAsync(context, _ => throw exception);

            // assert
            context.Response.StatusCode.Should().Be(500);
        }
    }
}