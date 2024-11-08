using dotenv.net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CityProviderContractTests;

public static class Startup
{
    public static IHostBuilder CreateHostBuilder()
    {
        DotEnv.Fluent().WithTrimValues().WithOverwriteExistingVars().Load();
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(
                (context, configuration) =>
                {
                    configuration.Sources.Clear();
                    configuration
                        .AddJsonFile($"appsettings.json", optional: false)
                        .AddEnvironmentVariables();
                }
            )
            .ConfigureServices(
                (context, services) =>
                {
                    services.AddCityProviderContractTests(context);
                }
            );
    }
}
