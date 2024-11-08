﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PactNet;
using WeatherForcast.Clients.CityProvider.V1;

namespace WeatherForcastContractTests;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeatherForcastContractTests(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        return services.AddCityProviderClient().AddPactConfiguration();
    }

    public static IServiceCollection AddCityProviderClient(this IServiceCollection services)
    {
        services.AddHttpClient<ICityProviderClient, CityProviderClient>(c =>
        {
            c.BaseAddress = new Uri(Constants.CityProviderBaseAddress);
        });

        return services;
    }

    public static IServiceCollection AddPactConfiguration(this IServiceCollection services)
    {
        services.AddSingleton(
            new PactConfig() { PactDir = Constants.PactDir, LogLevel = PactLogLevel.Error }
        );

        return services;
    }
}