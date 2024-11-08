using System.Text.Json;
using System.Text.Json.Serialization;
using MinimalApi.Endpoint.Extensions;
using WeatherForcast.Clients.CityProvider.V1;
using WeatherForcast.Clients.TemperatureProvider.V1;

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
        //services.ConfigureHttpJsonOptions(options =>
        //{
        //    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        //    options.SerializerOptions.WriteIndented = true;
        //    options.SerializerOptions.IncludeFields = true;
        //    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        //    options.SerializerOptions.PropertyNameCaseInsensitive = true;
        //    options.SerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        //});

        return services.AddEndpointsApiExplorer().AddSwaggerGen().AddEndpoints();
    }

    internal static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpClient<ICityProviderClient, CityProviderClient>(c =>
        {
            c.BaseAddress = new Uri($"https+http://{"cityprovider"}");
        });

        services.AddHttpClient<ITemperatureProviderClient, TemperatureProviderClient>(c =>
        {
            c.BaseAddress = new Uri($"https+http://{"temperatureprovider"}");
        });
        return services;
    }
}
