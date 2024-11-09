using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PactReferences;
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
        return services.AddCityServiceClient().AddPactReferences(context);
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
}
