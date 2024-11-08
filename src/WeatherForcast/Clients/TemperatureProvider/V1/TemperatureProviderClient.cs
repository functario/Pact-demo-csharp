using System.Net.Http;
using System.Text.Json;
using WeatherForcast.Clients.TemperatureProvider.V1.DTOs;

namespace WeatherForcast.Clients.TemperatureProvider.V1;

public sealed class TemperatureProviderClient : ITemperatureProviderClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public TemperatureProviderClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = false,
        };
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
            cancellationToken: cancellationToken
        );

        return temperatures ?? new GetTemperaturesResponse([]);
    }
}
