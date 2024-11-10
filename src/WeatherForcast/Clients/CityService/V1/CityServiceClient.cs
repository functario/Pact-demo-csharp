using System.Text.Json;
using DemoConfigurations;
using WeatherForcast.Clients.CityService.V1.DTOs;

namespace WeatherForcast.Clients.CityService.V1;

public sealed class CityServiceClient : ICityServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public CityServiceClient(HttpClient httpClient, DemoConfiguration demoConfiguration)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(demoConfiguration, nameof(demoConfiguration));
        _httpClient = httpClient;
        _jsonSerializerOptions = demoConfiguration.GetJsonSerializerOptions();
    }

    public string EndPoint => "v1/cities";

    public async Task<GetCitiesResponse> GetCities(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_httpClient.BaseAddress, nameof(_httpClient.BaseAddress));
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_httpClient.BaseAddress}{EndPoint}"
        );

        request.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(request, cancellationToken);

        var a = await response.Content.ReadAsStringAsync(cancellationToken);

        response.EnsureSuccessStatusCode();

        var cities = await response.Content.ReadFromJsonAsync<GetCitiesResponse>(
            _jsonSerializerOptions,
            cancellationToken
        );

        return cities ?? new GetCitiesResponse([]);
    }
}
