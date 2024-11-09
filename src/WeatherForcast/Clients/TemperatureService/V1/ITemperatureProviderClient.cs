using WeatherForcast.Clients.TemperatureService.V1.DTOs;

namespace WeatherForcast.Clients.TemperatureService.V1;

public interface ITemperatureProviderClient
{
    Task<GetTemperaturesResponse> GetTemperatures(CancellationToken cancellationToken);
    string TemperaturesEndPoint { get; }
}
