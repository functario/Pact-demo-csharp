using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoConfigurations;

public sealed class DemoConfiguration
{
    private readonly DemoCases _demoCases;

    public DemoConfiguration(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        _demoCases = configuration.GetValue<DemoCases>(EnvironmentVars.WEATHER_FORCAST_DEMO_CASE);
    }

    public JsonSerializerOptions GetJsonSerializerOptions()
    {
        return _demoCases switch
        {
            DemoCases.Undefined
                => throw new InvalidOperationException(
                    "DemoCases is undefined. Ensure that your '.env' is correctly configured."
                ),
            DemoCases.HappyPath => HappyPath(),
            DemoCases.JsonSerializerInvalidEnum => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
    }

    private static JsonSerializerOptions HappyPath()
    {
        var serializationOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = true,
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

        serializationOptions.Converters.Add(new JsonStringEnumConverter());
        return serializationOptions;
    }
}
