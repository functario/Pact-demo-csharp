﻿using CityServiceContractTests.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PactNet.Verifier;
using PactReferences;
using Xunit.Abstractions;

namespace CityServiceContractTests.V1;

public class GetCitiesTests
{
    private readonly DirectoryInfo _pactDir;
    private readonly PactVerifierConfig _config;
    private readonly WebApplication _cityService;

    public GetCitiesTests(
        [FromKeyedServices(Participants.CityService)] WebApplication cityService,
        ITestOutputHelper output
    )
    {
        // Initialize Rust backend
        _pactDir = PactConfigHelper.GetPactDir();
        _config = PactConfigHelper.GetPactVerifierConfig(output);
        _cityService = cityService;
    }

    [Fact]
    public void GetCities_ContractTests()
    {
        // Arrange
        using var pactVerifier = new PactVerifier(Participants.CityService, _config);
        var url = _cityService.Urls.Single();

        // Act, Assert
        pactVerifier
            .WithHttpEndpoint(new Uri(url))
            .WithDirectorySource(_pactDir)
            .WithProviderStateUrl(new Uri($"{url}/{ProviderStateMiddleware.ProviderStatesPath}"))
            .Verify();
    }
}