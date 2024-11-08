using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityProvider;
using CityProviderContractTests.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PactNet;
using PactNet.Output.Xunit;
using CityProviderStartup = CityProvider.Startup;

namespace CityProviderContractTests;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCityProviderContractTests(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        return services.AddCityProviderService();
    }

    public static IServiceCollection AddCityProviderService(this IServiceCollection services)
    {
        var server = CityProviderStartup.WebApp([]);

        // https://github.com/basdijkstra/introduction-to-contract-testing-dotnet/blob/609534f186c5f9e0fdc18da2389a61bdda22df66/AddressProvider.Tests/AddressPactTest.cs
        // To handle pact states.
        server.UseMiddleware<ProviderStateMiddleware>();

        server.Start();

        return services.AddActivatedSingleton(_ => server);
    }
}
