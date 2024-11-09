using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PactNet.Verifier;
using PactReferences;
using TemperatureServiceContractTests.Middlewares;
using Xunit.Abstractions;

namespace TemperatureServiceContractTests.V1;

public class GetTemperaturesTests
{
    private readonly DirectoryInfo _pactDir;
    private readonly PactVerifierConfig _config;
    private readonly WebApplication _temperatureService;

    public GetTemperaturesTests(
        [FromKeyedServices(Participants.TemperatureService)] WebApplication temperatureService,
        ITestOutputHelper output
    )
    {
        // Initialize Rust backend
        _pactDir = PactConfigHelper.GetPactDir();
        _config = PactConfigHelper.GetPactVerifierConfig(output);
        _temperatureService = temperatureService;
    }

    [Fact]
    public void GetTemperatures_ContractTests()
    {
        // Arrange
        using var pactVerifier = new PactVerifier(Participants.TemperatureService, _config);
        var url = _temperatureService.Urls.Single();

        // Act, Assert
        pactVerifier
            .WithHttpEndpoint(new Uri(url))
            .WithDirectorySource(_pactDir)
            .WithProviderStateUrl(new Uri($"{url}/{ProviderStateMiddleware.ProviderStatesPath}"))
            .Verify();
    }
}
