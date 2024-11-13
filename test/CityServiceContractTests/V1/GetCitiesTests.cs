using CityServiceContractTests.Middlewares;
using CityServiceContractTests.TestsConfigurations;
using DemoConfigurations;
using JWTGenerator;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PactNet.Verifier;
using PactReferences;
using Xunit.Abstractions;

namespace CityServiceContractTests.V1;

[CollectionDefinition(CityServiceContractTestsCollection.Name)]
public class GetCitiesTests
{
    private readonly DirectoryInfo _pactDir;
    private readonly PactVerifierConfig _config;
    private readonly WebApplication _cityService;
    private readonly DemoConfiguration _demoConfiguration;

    public GetCitiesTests(
        [FromKeyedServices(Participants.CityService)] WebApplication cityService,
        PactConfigHelper pactConfigHelper,
        DemoConfiguration demoConfiguration,
        ITestOutputHelper output
    )
    {
        ArgumentNullException.ThrowIfNull(pactConfigHelper, nameof(pactConfigHelper));

        // Initialize Rust backend
        _pactDir = pactConfigHelper.GetPactDir();
        _config = pactConfigHelper.GetPactVerifierConfig(output);
        _cityService = cityService;
        _demoConfiguration = demoConfiguration;
    }

    [Fact]
    public void GetCities_ContractTests()
    {
        // Arrange
        using var pactVerifier = new PactVerifier(Participants.CityService, _config);
        var url = _cityService.Urls.Single();

        // Generate a valid token
        // The solution with AuthorizationMiddleware does not seems to work:
        // https://github.com/pact-foundation/pact-workshop-dotnet?tab=readme-ov-file#step-9---implement-authorization-on-the-provider

        var token = TokenGenerator.GenerateToken(_demoConfiguration.AuthenticationKey);

        // Act, Assert
        pactVerifier
            .WithHttpEndpoint(new Uri(url))
            .WithDirectorySource(_pactDir)
            .WithCustomHeader("Authorization", $"Bearer {token}")
            .WithProviderStateUrl(new Uri($"{url}/{ProviderStateMiddleware.ProviderStatesPath}"))
            .Verify();
    }
}
