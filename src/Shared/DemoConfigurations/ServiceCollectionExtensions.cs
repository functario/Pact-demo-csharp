using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoConfigurations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDemoConfigurations(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        return services.ConfigureDemo(context);
    }

    public static IServiceCollection ConfigureDemo(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        services.AddSingleton(_ => new DemoConfiguration(context.Configuration));
        return services;
    }
}
