﻿using System.Net;
using System.Text.Json;
using DemoConfigurations;
using PactNet;
using PactNet.Matchers;
using PactReferences;
using PactReferences.ProviderStates;
using WeatherForcast.Clients.CityService.V1;
using WeatherForcast.Clients.CityService.V1.DTOs;
using WeatherForcastContractTests.Fixtures.AuthenticationServiceFixtures;
using WeatherForcastContractTests.Fixtures.CityServiceFixtures;
using Xunit.Abstractions;

namespace WeatherForcastContractTests.Tests.CityServiceTests.V1;

public class GetCitiesTests
{
    private readonly IPactBuilderV4 _pactBuilder;
    private readonly ICityServiceClient _cityServiceClient;
    private readonly VerifySettings _verifySettings;
    private readonly JsonSerializerOptions _demoJsonSerializerOptions;

    public GetCitiesTests(
        ICityServiceClient cityServiceClient,
        PactConfigHelper configHelper,
        DemoConfiguration demoConfiguration,
        VerifySettings verifySettings,
        ITestOutputHelper output
    )
    {
        ArgumentNullException.ThrowIfNull(configHelper, paramName: nameof(configHelper));
        ArgumentNullException.ThrowIfNull(demoConfiguration, nameof(demoConfiguration));

        var pact = Pact.V4(
            Participants.WeatherForcast,
            Participants.CityService,
            configHelper.GetPactConfig(output)
        );

        // Initialize Rust backend
        _pactBuilder = pact.WithHttpInteractions(port: Constants.CityServicePort);
        _cityServiceClient = cityServiceClient;
        _verifySettings = verifySettings;
        _demoJsonSerializerOptions = demoConfiguration.GetJsonSerializerOptions();
    }

    [Fact]
    public async Task GetCities_WhenSomeCitiesExist_ReturnsSomeCities()
    {
        // csharpier-ignore
        // Arrange
        var tokenMatch = Match.Regex(
            $"Bearer {AuthenticationFixtures.ValidOldToken}",
            "^Bearer\\s+([A-Za-z0-9\\-\\._~\\+\\/]+=*)$"
        );

        var expectedResponse = new GetCitiesResponse(CityFixtures.SetOf3Cities);

        // Body returns by API is lowerCase.
        var expectedBody = expectedResponse.ToLowerDynamic(_demoJsonSerializerOptions);

        _pactBuilder
            .UponReceiving("GetCities")
            .Given(CityServiceStates.SomeCitiesExist.State)
            .WithRequest(HttpMethod.Get, $"/{_cityServiceClient.EndPoint}")
            .WithHeader("Accept", "application/json")
            .WithHeader("Authorization", tokenMatch)
            .WillRespond()
            .WithStatus(HttpStatusCode.OK)
            .WithHeader("Content-Type", "application/json; charset=utf-8")
            .WithJsonBody(new TypeMatcher(expectedBody));

        // Act
        await _pactBuilder.VerifyAsync(async ctx =>
        {
            using var cancellationTokenSource = new CancellationTokenSource(
                TimeSpan.FromSeconds(5)
            );

            var response = await _cityServiceClient.GetCities(cancellationTokenSource.Token);

            // Assert
            await Verify(response, _verifySettings);
        });
    }
}
