﻿using MinimalApi.Endpoint.Extensions;

namespace CityProvider;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1052:Static holder types should be Static or NotInheritable",
    Justification = "<Pending>"
)]
public class Startup
{
    public static WebApplication WebApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Aspire
        builder.AddServiceDefaults();

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Logging.ClearProviders();

        // Register Services
        builder.Host.ConfigureServices((context, services) => services.AddCityProvider(context));

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
        return app;
    }
}
