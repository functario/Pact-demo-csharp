using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PactReferences;
using WeatherForcast.Clients.CityService.V1;
using WeatherForcast.Clients.TemperatureService.V1;
using WeatherForcastContractTests.TemperatureServiceTests.V1.Fakes;

namespace WeatherForcastContractTests;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeatherForcastContractTests(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        return services.AddClients().AddPactReferences(context);
    }

    public static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Register fake AuthorizationDelegatingHandler to inject token into the client.
        services.AddTransient<FakeAuthorizationDelegatingHandler>();

        services
            .AddHttpClient<ICityServiceClient, CityServiceClient>(
                (serviceProvider, client) =>
                {
                    client.BaseAddress = new Uri(Constants.CityServiceBaseAddress);
                }
            )
            .AddHttpMessageHandler<FakeAuthorizationDelegatingHandler>();

        services.AddHttpClient<ITemperatureServiceClient, TemperatureServiceClient>(
            (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(Constants.TemperatureServiceBaseAddress);
            }
        );

        return services;
    }
}
