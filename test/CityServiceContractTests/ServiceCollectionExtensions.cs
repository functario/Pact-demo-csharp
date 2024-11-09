using CityServiceContractTests.Middlewares;
using DemoConfigurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PactReferences;
using CityServiceStartup = CityService.Startup;

namespace CityServiceContractTests;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCityServiceContractTests(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        services.AddSingleton(_ => new DemoConfiguration(context.Configuration));
        return services.AddCityService();
    }

    public static IServiceCollection AddCityService(this IServiceCollection services)
    {
        var server = CityServiceStartup.WebApp([]);
        // To handle pact states.
        server.UseMiddleware<ProviderStateMiddleware>();
        server.Start();
        return services.AddActivatedKeyedSingleton(Participants.CityService, (_, _) => server);
    }
}
