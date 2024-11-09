using DemoConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PactNet;
using WeatherForcast.Clients.CityService.V1;
using WeatherForcast.Clients.TemperatureService.V1;

namespace WeatherForcastContractTests;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeatherForcastContractTests(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        services.AddSingleton(_ => new DemoConfiguration(context.Configuration));
        return services.AddCityServiceClient().AddPactConfiguration();
    }

    public static IServiceCollection AddCityServiceClient(this IServiceCollection services)
    {
        services.AddHttpClient<ICityServiceClient, CityServiceClient>(c =>
        {
            c.BaseAddress = new Uri(Constants.CityServiceBaseAddress);
        });

        services.AddHttpClient<ITemperatureServiceClient, TemperatureServiceClient>(c =>
        {
            c.BaseAddress = new Uri(Constants.TemperatureServiceBaseAddress);
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
