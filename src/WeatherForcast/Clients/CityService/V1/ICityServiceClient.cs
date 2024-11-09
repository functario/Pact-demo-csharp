using WeatherForcast.Clients.CityService.V1.DTOs;

namespace WeatherForcast.Clients.CityService.V1;

public interface ICityServiceClient
{
    Task<GetCitiesResponse> GetCities(CancellationToken cancellationToken);
    string EndPoint { get; }
}
