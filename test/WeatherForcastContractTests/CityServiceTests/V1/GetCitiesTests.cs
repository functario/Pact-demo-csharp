using System.Net;
using System.Text.Json;
using DemoConfigurations;
using FluentAssertions;
using PactNet;
using PactNet.Matchers;
using PactReferences;
using PactReferences.ProviderStates;
using WeatherForcast.Clients.CityService.V1;
using WeatherForcast.Clients.CityService.V1.DTOs;
using WeatherForcastContractTests.Fixtures.CityServiceFixtures;
using Xunit.Abstractions;

namespace WeatherForcastContractTests.CityServiceTests.V1;

public class GetCitiesTests
{
    private readonly IPactBuilderV4 _pactBuilder;
    private readonly ICityServiceClient _cityServiceClient;
    private readonly JsonSerializerOptions _demoJsonSerializerOptions;

    public GetCitiesTests(
        ICityServiceClient cityServiceClient,
        PactConfigHelper configHelper,
        DemoConfiguration demoConfiguration,
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
        _demoJsonSerializerOptions = demoConfiguration.GetJsonSerializerOptions();
    }

    [Fact]
    public async Task GetCities_WhenSomeCitiesExist_ReturnsSomeCities()
    {
        // Arrange
        var expectedResponse = new GetCitiesResponse(CityFixtures.SetOf3Cities);
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
