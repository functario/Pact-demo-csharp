using System.Text.Json;
using System.Text.Json.Serialization;
using CityService.Repositories;
using MinimalApi.Endpoint.Extensions;

namespace CityService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCityService(
        this IServiceCollection services,
        HostBuilderContext context,
        ICityRepository? injectedCityRepository
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        services.AddMinimalApi().AddRepositories(injectedCityRepository);

        return services;
    }

    internal static IServiceCollection AddRepositories(
        this IServiceCollection services,
        ICityRepository? injectedCityRepository
    )
    {
        // Inject the repository for test.
        // Probably safer than conditional environment since it must be instanciated.
        // Also the fake repository is not hardcoded in the source of the service.
        if (injectedCityRepository is not null)
        {
            return services.AddScoped(_ => injectedCityRepository);
        }

        return services.AddScoped<ICityRepository, CityRepository>();
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
