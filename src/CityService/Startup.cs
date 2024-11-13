using CityService.Repositories;
using dotenv.net;
using MinimalApi.Endpoint.Extensions;
using ServiceDefaults;

namespace CityService;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1052:Static holder types should be Static or NotInheritable",
    Justification = "<Pending>"
)]
public class Startup
{
    public static WebApplication WebApp(
        string[] args,
        ICityRepository? injectedCityRepository = null
    )
    {
        var builder = WebApplication.CreateBuilder(args);

        // Aspire
        builder.AddServiceDefaults();

        builder.Logging.ClearProviders();

        // Load appsettings and environment variables.
        DotEnv.Fluent().WithTrimValues().Load();
        builder
            .Configuration.AddJsonFile($"appsettings.json", optional: false)
            .AddEnvironmentVariables();

        // Register and Configure Services
        builder.Host.ConfigureServices(
            (context, services) => services.AddCityService(context, injectedCityRepository)
        );

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapEndpoints();
        app.MapDefaultEndpoints();
        app.UseExceptionHandler();

        // Middlewares
        // Todo: app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
