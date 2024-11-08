using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Matchers;
using PactNet.Output.Xunit;
using ProvidersPactStates;
using WeatherForcast.Clients.CityProvider.V1;
using WeatherForcast.Clients.CityProvider.V1.DTOs;
using WeatherForcast.Clients.CityProvider.V1.Models;
using Xunit.Abstractions;

namespace WeatherForcastContractTests.CityProviderTests.V1;

public class GetCitiesTests
{
    private readonly IPactBuilderV4 _pactBuilder;
    private readonly ICityProviderClient _cityProviderClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public GetCitiesTests(
        ICityProviderClient cityProviderClient,
        PactConfig pactConfig,
        ITestOutputHelper output
    )
    {
        ArgumentNullException.ThrowIfNull(pactConfig, nameof(pactConfig));
        pactConfig.Outputters = [new XunitOutput(output)];

        var pact = Pact.V4("WeatherForcast", "CityProvider", pactConfig);

        // Initialize Rust backend
        _pactBuilder = pact.WithHttpInteractions(port: Constants.CityProviderPort);
        _cityProviderClient = cityProviderClient;
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = false
        };
    }

    [Fact]
    public async Task GetCities_WhenSomeCitiesExist_ReturnsSomeCities()
    {
        // Arrange
        var expectedResponse = new GetCitiesResponse(Cities: [new City("Paris", "France")]);
        // Body returns by API is lowerCase.
        var expectedBody = expectedResponse.ToLowerDynamic();
        // csharpier-ignore
        _pactBuilder
            .UponReceiving("GetCities")
                .Given(CityProviderStates.SomeCitiesExist.State)
                .WithRequest(HttpMethod.Get, $"/{_cityProviderClient.CitiesEndPoint}")
                .WithHeader("Accept", "application/json")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new TypeMatcher(expectedBody));

        await _pactBuilder.VerifyAsync(async ctx =>
        {
            // Act
            var cancellationTokenSource = new CancellationTokenSource();
            var response = await _cityProviderClient.GetCities(cancellationTokenSource.Token);

            // Assert
            response.Should().BeEquivalentTo(expectedResponse);
        });
    }
}
