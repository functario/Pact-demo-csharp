using System.Net;
using FluentAssertions;
using PactNet;
using PactNet.Infrastructure.Outputters;
using WeatherForcast.Clients.CityProvider.V1;
using WeatherForcast.Clients.CityProvider.V1.DTOs;
using WeatherForcast.Clients.CityProvider.V1.Models;

namespace WeatherForcastContractTests.CityProviderTests.V1;

public class GetCitiesTests
{
    private readonly IPactBuilderV4 _pactBuilder;
    private readonly ICityProviderClient _cityProviderClient;

    public GetCitiesTests(ICityProviderClient cityProviderClient)
    {
        var pact = Pact.V4(
            "WeatherForcast",
            "CityProvider",
            new PactConfig()
            {
                LogLevel = PactLogLevel.Trace,
                Outputters = new List<IOutput> { new ConsoleOutput() }
            }
        );

        // Initialize Rust backend
        _pactBuilder = pact.WithHttpInteractions(port: 7429);
        _cityProviderClient = cityProviderClient;
    }

    [Fact]
    public async Task GetCities_WhenSomeCitiesExist_ReturnsSomeCities()
    {
        // Arrange
        var expectedBody = new GetCitiesResponse(Cities: [new City("Paris", "France")]);
        // csharpier-ignore
        _pactBuilder
            .UponReceiving("GetCities")
                .Given("Some cities exist")
                .WithRequest(HttpMethod.Get, "/v1/cities")
                .WithHeader("Accept", "application/json")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(expectedBody);

        await _pactBuilder.VerifyAsync(async ctx =>
        {
            // Act
            var cancellationTokenSource = new CancellationTokenSource();
            var response = await _cityProviderClient.GetCities(cancellationTokenSource.Token);

            // Assert
            response.Should().BeEquivalentTo(expectedBody);
        });
    }
}
