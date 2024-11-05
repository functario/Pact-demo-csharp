using MinimalApi.Endpoint.Extensions;
using WeatherForcast.Clients.CityProvider.V1;

namespace WeatherForcast;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeatherForcast(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        services.AddMinimalApi();
        services.AddClients();

        return services;
    }

    internal static IServiceCollection AddMinimalApi(this IServiceCollection services)
    {
        return services.AddEndpointsApiExplorer().AddSwaggerGen().AddEndpoints();
    }

    internal static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpClient<ICityProviderClient, CityProviderClient>(c =>
        {
            c.BaseAddress = new Uri($"https+http://{"cityprovider"}");
        });
        return services;
    }
}
