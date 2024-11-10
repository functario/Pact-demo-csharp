using System.Text.Json;
using System.Text.Json.Serialization;
using DemoConfigurations.Configurations;
using Microsoft.Extensions.Options;
using PactNet;

namespace DemoConfigurations;

public sealed class DemoConfiguration
{
    private readonly DemoCases _demoCase;
    private readonly PactLogLevel _pactLogLevel;
    private readonly string _pactFolder;
    private readonly string _authenticationKey;

    public DemoConfiguration(IOptionsMonitor<DemoOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        var pactFolder = options.CurrentValue.PactFolder;
        var authenticationKey = options.CurrentValue.AuthenticationKey;
        var demoCase = options.CurrentValue.DemoCase;
        var pactLogLevel = options.CurrentValue.PactLogLevel;
        ArgumentException.ThrowIfNullOrWhiteSpace(pactFolder, EnvironmentVars.PACTDEMO_PACTFOLDER);
        ArgumentException.ThrowIfNullOrWhiteSpace(
            authenticationKey,
            EnvironmentVars.PACTDEMO_AUTHENTICATIONKEY
        );

        _pactFolder = pactFolder;
        _authenticationKey = authenticationKey;
        _demoCase = demoCase;
        _pactLogLevel = pactLogLevel;
    }

    public PactLogLevel PactLogLevel => _pactLogLevel;
    public string PactFolder => _pactFolder;
    public string AuthenticationKey => _authenticationKey;

    public JsonSerializerOptions GetJsonSerializerOptions()
    {
        return _demoCase switch
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
