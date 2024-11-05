using WeatherForcast.Clients.CityProvider.V1.DTOs;

namespace WeatherForcast.Clients.CityProvider.V1;

public sealed class CityProviderClient : ICityProviderClient
{
    private readonly HttpClient _httpClient;

    public CityProviderClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetCitiesResponse> GetCities(CancellationToken cancellationToken)
    {
        var uri = new Uri($"{_httpClient.BaseAddress}v1/cities");
        var response = await _httpClient.GetAsync(uri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var cities = await response.Content.ReadFromJsonAsync<GetCitiesResponse>(
            cancellationToken: cancellationToken
        );

        return cities ?? new GetCitiesResponse([]);
    }
}
