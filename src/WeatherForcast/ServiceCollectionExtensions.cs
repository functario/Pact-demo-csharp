using DemoConfigurations;
using MinimalApi.Endpoint.Extensions;
using WeatherForcast.Clients.CityProvider.V1;
using WeatherForcast.Clients.TemperatureProvider.V1;

namespace WeatherForcast;

public static class ServiceCollectionExtensions
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static DemoConfiguration s_demoConfiguration;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public static IServiceCollection AddWeatherForcast(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        s_demoConfiguration = new DemoConfiguration(context.Configuration);
        services.AddMinimalApi();
        services.AddClients();
        services.AddSingleton(_ => s_demoConfiguration);

        return services;
    }

    internal static IServiceCollection AddMinimalApi(this IServiceCollection services)
    {
        var serializationOptions = s_demoConfiguration.GetJsonSerializerOptions();

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy =
                serializationOptions.PropertyNamingPolicy;
            options.SerializerOptions.WriteIndented = serializationOptions.WriteIndented;
            options.SerializerOptions.IncludeFields = serializationOptions.IncludeFields;
            foreach (var converter in serializationOptions.Converters)
            {
                options.SerializerOptions.Converters.Add(converter);
            }

            options.SerializerOptions.PropertyNameCaseInsensitive =
                serializationOptions.PropertyNameCaseInsensitive;
            options.SerializerOptions.ReadCommentHandling =
                serializationOptions.ReadCommentHandling;
        });

        return services.AddEndpointsApiExplorer().AddSwaggerGen().AddEndpoints();
    }

    internal static IServiceCollection AddClients(this IServiceCollection services)
    {
        var jsonSerializerOptions = s_demoConfiguration.GetJsonSerializerOptions();
        services.AddHttpClient();
        services.AddHttpClient<ICityProviderClient, CityProviderClient>(c =>
        {
            c.BaseAddress = new Uri($"https+http://{"cityprovider"}");
        });

        services.AddHttpClient<ITemperatureProviderClient, TemperatureProviderClient>(c =>
        {
            c.BaseAddress = new Uri($"https+http://{"temperatureservice"}");
        });

        return services;
    }
}
