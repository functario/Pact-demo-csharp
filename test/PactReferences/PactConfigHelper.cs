using DemoConfigurations;
using PactNet;
using PactNet.Output.Xunit;
using PactNet.Verifier;
using Xunit.Abstractions;

namespace PactReferences;

public class PactConfigHelper
{
    private readonly string _relativePactDir;
    private readonly PactLogLevel _pactLogLevel;

    public PactConfigHelper(DemoConfiguration demoConfiguration)
    {
        ArgumentNullException.ThrowIfNull(demoConfiguration, nameof(demoConfiguration));
        _relativePactDir = demoConfiguration.PactFolder;
        _pactLogLevel = demoConfiguration.PactLogLevel;
    }

    public DirectoryInfo GetPactDir()
    {
        var pactDir = new DirectoryInfo(_relativePactDir);
        if (!Directory.Exists(pactDir.FullName))
        {
            throw new DirectoryNotFoundException($"Directory {pactDir.FullName} does not exist.");
        }

        return pactDir;
    }

    public PactConfig GetPactConfig(ITestOutputHelper output)
    {
        return new PactConfig
        {
            Outputters = [new XunitOutput(output)],
            LogLevel = _pactLogLevel,
            PactDir = GetPactDir().FullName
        };
    }

    public PactVerifierConfig GetPactVerifierConfig(ITestOutputHelper output)
    {
        return new PactVerifierConfig
        {
            Outputters = [new XunitOutput(output)],
            LogLevel = _pactLogLevel
        };
    }
}
