using MinimalApi.Endpoint.Extensions;
using ServiceDefaults;
using TemperatureService.Repositories;

namespace TemperatureService;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1052:Static holder types should be Static or NotInheritable",
    Justification = "<Pending>"
)]
public class Startup
{
    public static WebApplication WebApp(
        string[] args,
        ITemperatureRepository? temperatureRepository = null
    )
    {
        var builder = WebApplication.CreateBuilder(args);

        // Aspire
        builder.AddServiceDefaults();

        builder.Logging.ClearProviders();

        // Register Services
        builder.Host.ConfigureServices(
            (context, services) => services.AddTemperatureService(context, temperatureRepository)
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

        return app;
    }
}
