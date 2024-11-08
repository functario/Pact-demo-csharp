using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace CityProviderContractTests;

public static class Startup
{
    public static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(
                (context, configuration) =>
                {
                    configuration.Sources.Clear();
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
