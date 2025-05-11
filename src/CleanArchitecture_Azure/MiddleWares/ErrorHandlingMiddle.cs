
using Restaurants.Domain.Exceptions;

namespace CleanArchitecture_Azure.MiddleWares
{
    //public class ErrorHandlingMiddle(ILogger<ErrorHandlingMiddle> logger) : IMiddleware
    //{
    //	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    //	{
    //		try
    //		{
    //			await next.Invoke(context);
    //		}
    //		catch (NotFoundException ex)
    //		{
    //			context.Response.StatusCode = 404;
    //			await context.Response.WriteAsync(ex.Message);

    //			logger.LogWarning(ex.Message);
    //		}
    //		catch (Exception ex)
    //		{
    //			logger.LogError(ex, ex.Message);
    //			context.Response.StatusCode = 500;
    //			await context.Response.WriteAsync("Some thing went wrong");
    //		}
    //	}
    //}
    public class ErrorHandlingMiddle(ILogger<ErrorHandlingMiddle> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"{{ \"error\": \"{ex.Message}\" }}");
                logger.LogWarning("NotFoundException: {Message}", ex.Message);
            }
            catch (ForbidException)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Access Forbidden");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled Exception: {Message}", ex.Message);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync($$"""
            {
                "error": "Internal Server Error",
                "message": "{{ex.Message}}"
            }
            """);
            }
        }
    }

}
