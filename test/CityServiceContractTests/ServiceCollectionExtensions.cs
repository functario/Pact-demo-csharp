using CityService;
using CityService.Authentications;
using CityService.Repositories;
using CityServiceContractTests.Middlewares;
using DemoConfigurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
        return services.AddPactReferences(context).AddCityService(context);
    }

    public static IServiceCollection AddCityService(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        // repository use for testing with Pact States
        var fakeRepository = new FakeCityRepository();
        services.AddScoped<ICityRepository>(_ => fakeRepository);

        var disableAuthorizationPolicy = GetDemoCityAuthorizationPolicy(context);

        var startupOptions = new StartupOptions(fakeRepository, disableAuthorizationPolicy);

        var server = CityServiceStartup.WebApp([], startupOptions);

        // To handle pact states.
        server.UseMiddleware<ProviderStateMiddleware>();
        // TODO: Investigate why it is not called if add after ProviderStateMiddleware.
        // Also it is only called on "provider-states" and not other called.
        //server.UseMiddleware<AuthorizationMiddleware>();
        server.Start();
        return services.AddActivatedKeyedSingleton(Participants.CityService, (_, _) => server);
    }

    private static CityAuthorizationPolicy? GetDemoCityAuthorizationPolicy(
        HostBuilderContext context
    )
    {
        var disableCityServiceAuthorization = context.Configuration.GetValue<bool>(
            EnvironmentVars.PACTDEMO_DISABLE_CITYSERVICE_AUTHORIZATION
        );

        if (disableCityServiceAuthorization)
        {
            return new CityAuthorizationPolicy(
                "testcitypolicy",
                policy => policy.RequireAssertion(context => true)
            );
        }

        return null;
    }
}
