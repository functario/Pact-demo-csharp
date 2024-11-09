using System.Text.Json;
using DemoConfigurations;
using WeatherForcast.Clients.TemperatureService.V1.DTOs;

namespace WeatherForcast.Clients.TemperatureService.V1;

public sealed class TemperatureProviderClient : ITemperatureProviderClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public TemperatureProviderClient(HttpClient httpClient, DemoConfiguration demoConfiguration)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(demoConfiguration, nameof(demoConfiguration));
        _httpClient = httpClient;
        _jsonSerializerOptions = demoConfiguration.GetJsonSerializerOptions();
    }

    public string TemperaturesEndPoint => "v1/temperatures";

    public async Task<GetTemperaturesResponse> GetTemperatures(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_httpClient.BaseAddress, nameof(_httpClient.BaseAddress));
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_httpClient.BaseAddress}{TemperaturesEndPoint}"
        );

        request.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var temperatures = await response.Content.ReadFromJsonAsync<GetTemperaturesResponse>(
            _jsonSerializerOptions,
            cancellationToken
        );

        return temperatures ?? new GetTemperaturesResponse([]);
    }
}
