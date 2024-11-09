using DemoConfigurations;
using MinimalApi.Endpoint.Extensions;
using WeatherForcast.Clients.CityService.V1;
using WeatherForcast.Clients.TemperatureService.V1;

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
        services.AddDemoConfigurations(context);
        return services;
    }

    internal static IServiceCollection AddMinimalApi(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            var demoConfiguration = services
                .BuildServiceProvider()
                .GetRequiredService<DemoConfiguration>();

            var demoOptions = demoConfiguration.GetJsonSerializerOptions();
            options.SerializerOptions.PropertyNamingPolicy = demoOptions.PropertyNamingPolicy;
            options.SerializerOptions.WriteIndented = demoOptions.WriteIndented;
            options.SerializerOptions.IncludeFields = demoOptions.IncludeFields;
            foreach (var converter in demoOptions.Converters)
            {
                options.SerializerOptions.Converters.Add(converter);
            }

            options.SerializerOptions.PropertyNameCaseInsensitive =
                demoOptions.PropertyNameCaseInsensitive;
            options.SerializerOptions.ReadCommentHandling = demoOptions.ReadCommentHandling;
        });

        return services.AddEndpointsApiExplorer().AddSwaggerGen().AddEndpoints();
    }

    internal static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpClient<ICityServiceClient, CityServiceClient>(c =>
        {
            c.BaseAddress = new Uri($"https+http://{"cityservice"}");
        });

        services.AddHttpClient<ITemperatureServiceClient, TemperatureServiceClient>(c =>
        {
            c.BaseAddress = new Uri($"https+http://{"temperatureservice"}");
        });

        return services;
    }
}
