using Microsoft.AspNetCore.Builder;
using PactNet.Verifier;
using Xunit.Abstractions;

namespace CityServiceContractTests.V1;

public class GetCitiesTests
{
    private readonly DirectoryInfo _pactDir;
    private readonly PactVerifierConfig _config;
    private readonly WebApplication _cityServiceService;

    public GetCitiesTests(WebApplication cityServiceService, ITestOutputHelper output)
    {
        // Initialize Rust backend
        _pactDir = PactHelper.GetPactDir();
        _config = PactHelper.GetPactVerifierConfig(output);
        _cityServiceService = cityServiceService;
    }

    [Fact]
    public void GetCities_ContractTests()
    {
        // Arrange
        using var pactVerifier = new PactVerifier(Constants.CityService, _config);
        var url = _cityServiceService.Urls.Single();

        // Act, Assert

        // https://github.com/pact-foundation/pact-workshop-dotnet/blob/b724b38efcee8de00a40ab13b7f3c21681f015ff/Provider/tests/Middleware/ProviderState.cs

        pactVerifier
            .WithHttpEndpoint(new Uri(url))
            .WithDirectorySource(_pactDir)
            .WithProviderStateUrl(new Uri($"{url}/provider-states"))
            .Verify();
    }
}
