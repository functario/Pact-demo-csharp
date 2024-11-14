using CityService;
using CityService.Repositories;
using CityServiceContractTests.Middlewares;
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
        return services.AddCityService().AddPactReferences(context);
    }

    public static IServiceCollection AddCityService(this IServiceCollection services)
    {
        services.AddScoped<ICityRepository>(_ => new FakeCityRepository());

        var startupOptions = new StartupOptions(
            services.BuildServiceProvider().GetRequiredService<ICityRepository>()
        );

        var server = CityServiceStartup.WebApp([], startupOptions);

        // To handle pact states.
        server.UseMiddleware<ProviderStateMiddleware>();
        // TODO: Investigate why it is not called if add after ProviderStateMiddleware.
        // Also it is only called on "provider-states" and not other called.
        //server.UseMiddleware<AuthorizationMiddleware>();
        server.Start();
        return services.AddActivatedKeyedSingleton(Participants.CityService, (_, _) => server);
    }
}
