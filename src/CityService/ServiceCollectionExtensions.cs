using System.Text.Json;
using System.Text.Json.Serialization;
using CityService.Repositories;
using MinimalApi.Endpoint.Extensions;

namespace CityService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCityService(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        services.AddMinimalApi().AddRepositories();

        return services;
    }

    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<ICityRepository, FakeCityRepository>();
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
