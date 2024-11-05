using MinimalApi.Endpoint.Extensions;

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

        return services;
    }

    internal static IServiceCollection AddMinimalApi(this IServiceCollection services)
    {
        return services.AddEndpointsApiExplorer().AddSwaggerGen().AddEndpoints();
    }
}
