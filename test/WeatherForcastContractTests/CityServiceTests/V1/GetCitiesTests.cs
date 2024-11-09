using System.Net;
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
    private readonly ICityServiceClient _cityProviderClient;

    public GetCitiesTests(
        ICityServiceClient cityProviderClient,
        PactConfig pactConfig,
        ITestOutputHelper output
    )
    {
        ArgumentNullException.ThrowIfNull(pactConfig, nameof(pactConfig));
        pactConfig.Outputters = [new XunitOutput(output)];

        var pact = Pact.V4(Participants.WeatherForcast, Participants.CityService, pactConfig);

        // Initialize Rust backend
        _pactBuilder = pact.WithHttpInteractions(port: Constants.CityServicePort);
        _cityProviderClient = cityProviderClient;
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
                .Given(CityServiceStates.SomeCitiesExist.State)
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
