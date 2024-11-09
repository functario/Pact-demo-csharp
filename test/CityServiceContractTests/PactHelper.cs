using PactNet;
using PactNet.Output.Xunit;
using PactNet.Verifier;
using Xunit.Abstractions;

namespace CityServiceContractTests;

internal static class PactHelper
{
    public static DirectoryInfo GetPactDir()
    {
        var pactDir = new DirectoryInfo(Constants.PactDir);
        if (!Directory.Exists(pactDir.FullName))
        {
            throw new DirectoryNotFoundException($"Directory {pactDir.FullName} does not exist.");
        }

        return pactDir;
    }

    public static PactVerifierConfig GetPactVerifierConfig(ITestOutputHelper output)
    {
        return new PactVerifierConfig
        {
            Outputters = [new XunitOutput(output)],
            LogLevel = PactLogLevel.Debug
        };
    }

    public static PactVerifierConfig GetPactVerifier(ITestOutputHelper output)
    {
        return new PactVerifierConfig
        {
            Outputters = [new XunitOutput(output)],
            LogLevel = PactLogLevel.Debug
        };
    }
}
