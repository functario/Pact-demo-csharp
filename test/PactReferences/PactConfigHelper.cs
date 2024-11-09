using PactNet;
using PactNet.Output.Xunit;
using PactNet.Verifier;
using Xunit.Abstractions;

namespace PactReferences;

public static class PactConfigHelper
{
    /// <summary>
    /// PACT contracts folder relative to the csproject of the test projects bin folder.
    /// </summary>
    private static string PactDir => Path.Combine("../../../../pacts");

    public static DirectoryInfo GetPactDir()
    {
        var pactDir = new DirectoryInfo(PactDir);
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
            LogLevel = PactLogLevel.Error
        };
    }
}
