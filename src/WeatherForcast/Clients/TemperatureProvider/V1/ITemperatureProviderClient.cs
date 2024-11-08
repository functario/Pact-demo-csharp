using WeatherForcast.Clients.TemperatureProvider.V1.DTOs;

namespace WeatherForcast.Clients.TemperatureProvider.V1;

public interface ITemperatureProviderClient
{
    Task<GetTemperaturesResponse> GetTemperatures(CancellationToken cancellationToken);
    string TemperaturesEndPoint { get; }
}
