using DemoConfigurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PactReferences;
using TemperatureServiceContractTests.Middlewares;
using TemperatureServiceStartup = TemperatureService.Startup;

namespace TemperatureServiceContractTests;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemperatureServiceContractTests(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        services.AddSingleton(_ => new DemoConfiguration(context.Configuration));
        return services.AddTemperatureService();
    }

    public static IServiceCollection AddTemperatureService(this IServiceCollection services)
    {
        var server = TemperatureServiceStartup.WebApp([]);
        server.UseMiddleware<ProviderStateMiddleware>();
        server.Start();
        return services.AddActivatedKeyedSingleton(
            Participants.TemperatureService,
            (_, _) => server
        );
    }
}
