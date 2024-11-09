using DemoConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PactReferences;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPactReferences(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        services.AddScoped<PactConfigHelper>();
        services.AddDemoConfigurations(context);
        return services;
    }
}
