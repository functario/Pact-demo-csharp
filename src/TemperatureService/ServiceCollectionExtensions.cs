using System.Text.Json;
using System.Text.Json.Serialization;
using MinimalApi.Endpoint.Extensions;
using TemperatureService.Repositories;

namespace TemperatureService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemperatureService(
        this IServiceCollection services,
        HostBuilderContext context,
        ITemperatureRepository? temperatureRepository
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        services.AddMinimalApi().AddRepositories(temperatureRepository);

        return services;
    }

    internal static IServiceCollection AddRepositories(
        this IServiceCollection services,
        ITemperatureRepository? injectedTemperatureRepository
    )
    {
        // Inject the repository for test.
        // Probably safer than conditional environment since it must be instanciated.
        // Also the fake repository is not hardcoded in the source of the service.
        if (injectedTemperatureRepository is not null)
        {
            return services.AddScoped(_ => injectedTemperatureRepository);
        }

        return services.AddScoped<ITemperatureRepository, TemperatureRepository>();
    }

    internal static IServiceCollection AddMinimalApi(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.IncludeFields = true;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        });

        return services.AddEndpointsApiExplorer().AddSwaggerGen().AddEndpoints();
    }
}
