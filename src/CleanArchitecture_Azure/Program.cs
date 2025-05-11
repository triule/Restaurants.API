using Restaurants.Domain.Entities;
using Restaurants.Application.Extensions;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using CleanArchitecture_Azure.MiddleWares;
using CleanArchitecture_Azure.Extentions;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.AddPresentation();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();
    builder.Logging.AddConsole();
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });
    var app = builder.Build();

    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
    try
    {
        await seeder.Seed();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Seeder Error: " + ex.Message);
        throw;
    }

    // Configure the HTTP request pipeline.
    app.UseMiddleware<ErrorHandlingMiddle>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.UseHttpsRedirection();

    app.MapGroup("/api/identity").MapIdentityApi<User>();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start up failed");
}
finally
{
    Log.CloseAndFlush();
}



public partial class Program { }