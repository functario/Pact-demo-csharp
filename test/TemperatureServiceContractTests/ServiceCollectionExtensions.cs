using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PactReferences;
using TemperatureService.Repositories;
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
        return services.AddTemperatureService().AddPactReferences(context);
    }

    public static IServiceCollection AddTemperatureService(this IServiceCollection services)
    {
        services.AddScoped(_ => TimeProvider.System);
        services.AddScoped<ITemperatureRepository>(_ => new FakeTemperatureRepository(
            services.BuildServiceProvider().GetRequiredService<TimeProvider>()
        ));

        var server = TemperatureServiceStartup.WebApp(
            [],
            services.BuildServiceProvider().GetRequiredService<ITemperatureRepository>()
        );

        server.UseMiddleware<ProviderStateMiddleware>();
        server.Start();
        return services.AddActivatedKeyedSingleton(
            Participants.TemperatureService,
            (_, _) => server
        );
    }
}
