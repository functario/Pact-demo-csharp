using System.Net;
using System.Text.Json;
using DemoConfigurations;
using FluentAssertions;
using PactNet;
using PactNet.Matchers;
using PactReferences;
using PactReferences.ProviderStates;
using WeatherForcast.Clients.TemperatureService.V1;
using WeatherForcast.Clients.TemperatureService.V1.DTOs;
using WeatherForcastContractTests.Fixtures.TemperatureServiceFixtures;
using Xunit.Abstractions;

namespace WeatherForcastContractTests.TemperatureServiceTests.V1;

public class GetTemperaturesTests
{
    private readonly IPactBuilderV4 _pactBuilder;
    private readonly ITemperatureServiceClient _temperatureServiceClient;
    private readonly JsonSerializerOptions _demoJsonSerializerOptions;

    public GetTemperaturesTests(
        ITemperatureServiceClient temperatureServiceClient,
        PactConfigHelper configHelper,
        DemoConfiguration demoConfiguration,
        ITestOutputHelper output
    )
    {
        ArgumentNullException.ThrowIfNull(configHelper, paramName: nameof(configHelper));
        ArgumentNullException.ThrowIfNull(demoConfiguration, nameof(demoConfiguration));

        var pact = Pact.V4(
            Participants.WeatherForcast,
            Participants.TemperatureService,
            configHelper.GetPactConfig(output)
        );

        // Initialize Rust backend
        _pactBuilder = pact.WithHttpInteractions(port: Constants.TemperatureServicePort);
        _temperatureServiceClient = temperatureServiceClient;
        _demoJsonSerializerOptions = demoConfiguration.GetJsonSerializerOptions();
    }

    [Fact]
    public async Task GetTemperatures_WhenSomeTemperatureExist_ReturnsSomeTemperatures()
    {
        // Arrange

        var expectedResponse = new GetTemperaturesResponse(
            TemperatureFixtures.SetOf3CelsiusTemperatures
        );
        // Body returns by API is lowerCase.
        var expectedBody = expectedResponse.ToLowerDynamic(_demoJsonSerializerOptions);
        // csharpier-ignore
        _pactBuilder
            .UponReceiving("GetTemperatures")
                .Given(TemperatureServiceStates.SomeTemperaturesExist.State)
                .WithRequest(HttpMethod.Get, $"/{_temperatureServiceClient.EndPoint}")
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
            var response = await _temperatureServiceClient.GetTemperatures(
                cancellationTokenSource.Token
            );

            // Assert
            response.Should().BeEquivalentTo(expectedResponse);
        });
    }
}
