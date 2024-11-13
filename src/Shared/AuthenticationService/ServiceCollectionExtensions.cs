using System.Text.Json;
using System.Text.Json.Serialization;
using DemoConfigurations;
using MinimalApi.Endpoint.Extensions;

namespace AuthenticationService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthenticationService(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        services.AddMinimalApi().AddDemoConfigurations(context).AddSwagger();
        return services;
    }

    internal static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c => { });
    }

    internal static IServiceCollection AddMinimalApi(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.IncludeFields = true;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        });

        return services.AddEndpointsApiExplorer().AddSwaggerGen().AddEndpoints();
    }
}
