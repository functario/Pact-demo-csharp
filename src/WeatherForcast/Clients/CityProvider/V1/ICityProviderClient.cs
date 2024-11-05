using WeatherForcast.Clients.CityProvider.V1.DTOs;

namespace WeatherForcast.Clients.CityProvider.V1;

public interface ICityProviderClient
{
    Task<GetCitiesResponse> GetCities(CancellationToken cancellationToken);
    string CitiesEndPoint { get; }
}
