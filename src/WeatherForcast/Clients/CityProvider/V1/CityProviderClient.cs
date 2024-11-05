﻿using System.Net.Http;
using WeatherForcast.Clients.CityProvider.V1.DTOs;

namespace WeatherForcast.Clients.CityProvider.V1;

public sealed class CityProviderClient : ICityProviderClient
{
    private readonly HttpClient _httpClient;

    public CityProviderClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string CitiesEndPoint => "v1/cities";

    public async Task<GetCitiesResponse> GetCities(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_httpClient.BaseAddress, nameof(_httpClient.BaseAddress));
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_httpClient.BaseAddress}{CitiesEndPoint}"
        );

        request.Headers.Add("Accept", "application/json");

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var cities = await response.Content.ReadFromJsonAsync<GetCitiesResponse>(
            cancellationToken: cancellationToken
        );

        return cities ?? new GetCitiesResponse([]);
    }
}
