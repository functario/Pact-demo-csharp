﻿using System.Net;
using System.Text.Json;
using DemoConfigurations;
using FluentAssertions;
using PactNet;
using PactNet.Matchers;
using PactNet.Output.Xunit;
using PactReferences;
using PactReferences.ProviderStates;
using WeatherForcast.Clients.CityService.V1;
using WeatherForcast.Clients.CityService.V1.DTOs;
using WeatherForcast.Clients.CityService.V1.Models;
using Xunit.Abstractions;

namespace WeatherForcastContractTests.CityServiceTests.V1;

public class GetCitiesTests
{
    private readonly IPactBuilderV4 _pactBuilder;
    private readonly ICityServiceClient _cityServiceClient;
    private readonly JsonSerializerOptions _demoJsonSerializerOptions;

    public GetCitiesTests(
        ICityServiceClient cityServiceClient,
        PactConfig pactConfig,
        DemoConfiguration demoConfiguration,
        ITestOutputHelper output
    )
    {
        ArgumentNullException.ThrowIfNull(pactConfig, nameof(pactConfig));
        ArgumentNullException.ThrowIfNull(demoConfiguration, nameof(demoConfiguration));
        pactConfig.Outputters = [new XunitOutput(output)];

        var pact = Pact.V4(Participants.WeatherForcast, Participants.CityService, pactConfig);

        // Initialize Rust backend
        _pactBuilder = pact.WithHttpInteractions(port: Constants.CityServicePort);
        _cityServiceClient = cityServiceClient;
        _demoJsonSerializerOptions = demoConfiguration.GetJsonSerializerOptions();
    }

    [Fact]
    public async Task GetCities_WhenSomeCitiesExist_ReturnsSomeCities()
    {
        // Arrange
        var expectedResponse = new GetCitiesResponse(Cities: [new City("Paris", "France")]);
        // Body returns by API is lowerCase.
        var expectedBody = expectedResponse.ToLowerDynamic(_demoJsonSerializerOptions);
        // csharpier-ignore
        _pactBuilder
            .UponReceiving("GetCities")
                .Given(CityServiceStates.SomeCitiesExist.State)
                .WithRequest(HttpMethod.Get, $"/{_cityServiceClient.EndPoint}")
                .WithHeader("Accept", "application/json")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new TypeMatcher(expectedBody));

        await _pactBuilder.VerifyAsync(async ctx =>
        {
            // Act
            using var cancellationTokenSource = new CancellationTokenSource(
                TimeSpan.FromSeconds(5)
            );
            var response = await _cityServiceClient.GetCities(cancellationTokenSource.Token);

            // Assert
            response.Should().BeEquivalentTo(expectedResponse);
        });
    }
}
