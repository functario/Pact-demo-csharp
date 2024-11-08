using MinimalApi.Endpoint.Extensions;

namespace TemperatureProvider;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemperatureProvider(
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
