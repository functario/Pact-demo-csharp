using DemoConfigurations.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PactNet;

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
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        var configuration = context.Configuration;
        services
            .AddOptions<DemoOptions>()
            .BindConfiguration(DemoOptions.Section)
            .Configure(x =>
            {
                var authenticationKey = configuration.GetValue<string>(
                    EnvironmentVars.PACTDEMO_AUTHENTICATIONKEY
                );
                var pactFolder = configuration.GetValue<string?>(
                    EnvironmentVars.PACTDEMO_PACTFOLDER
                );
                ArgumentException.ThrowIfNullOrEmpty(authenticationKey);
                ArgumentException.ThrowIfNullOrEmpty(pactFolder);

                x.DemoCase = configuration.GetValue<DemoCases>(EnvironmentVars.PACTDEMO_DEMOCASE);
                x.AuthenticationKey = authenticationKey;
                x.PactFolder = pactFolder;
                x.PactLogLevel = configuration.GetValue<PactLogLevel>(
                    EnvironmentVars.PACTDEMO_PACTLOGLEVEL
                );
            })
            .ValidateOnStart();

        services.AddSingleton<DemoConfiguration>();
        return services;
    }
}
