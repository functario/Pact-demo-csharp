using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Output.Xunit;
using PactNet.Verifier;
using Xunit;
using Xunit.Abstractions;
using static Google.Protobuf.Compiler.CodeGeneratorResponse.Types;

namespace CityProviderContractTests.V1;

public class GetCitiesTests
{
    private readonly DirectoryInfo _pactDir;
    private readonly IPactBuilderV4 _pactBuilder;
    private readonly PactVerifierConfig _config;
    private readonly WebApplication _cityProviderService;

    public GetCitiesTests(WebApplication cityProviderService, ITestOutputHelper output)
    {
        var pact = Pact.V4("WeatherForcast", "CityProvider");

        // Initialize Rust backend
        _pactDir = PactHelper.GetPactDir();

        _pactBuilder = pact.WithHttpInteractions(port: Constants.CityProviderPort);
        _config = PactHelper.GetPactVerifierConfig(output);
        _cityProviderService = cityProviderService;
    }

    [Fact]
    public void GetCities_ContractTests()
    {
        // Arrange
        using var pactVerifier = new PactVerifier(Constants.CityProvider, _config);
        var url = _cityProviderService.Urls.Single();

        // Act, Assert

        // https://github.com/pact-foundation/pact-workshop-dotnet/blob/b724b38efcee8de00a40ab13b7f3c21681f015ff/Provider/tests/Middleware/ProviderState.cs

        pactVerifier
            .WithHttpEndpoint(new Uri(url))
            .WithDirectorySource(_pactDir)
            .WithProviderStateUrl(new Uri($"{url}/provider-states"))
            .Verify();
    }
}
