using CityServiceContractTests.Middleware;
using DemoConfigurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        return services.AddCityServiceService();
    }

    public static IServiceCollection AddCityServiceService(this IServiceCollection services)
    {
        var server = CityServiceStartup.WebApp([]);

        // https://github.com/basdijkstra/introduction-to-contract-testing-dotnet/blob/609534f186c5f9e0fdc18da2389a61bdda22df66/AddressProvider.Tests/AddressPactTest.cs
        // To handle pact states.
        server.UseMiddleware<ProviderStateMiddleware>();

        server.Start();

        return services.AddActivatedSingleton(_ => server);
    }
}
