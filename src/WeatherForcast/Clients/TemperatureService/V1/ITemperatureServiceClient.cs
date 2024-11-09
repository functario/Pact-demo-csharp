using WeatherForcast.Clients.TemperatureService.V1.DTOs;

namespace WeatherForcast.Clients.TemperatureService.V1;

public interface ITemperatureServiceClient
{
    Task<GetTemperaturesResponse> GetTemperatures(CancellationToken cancellationToken);
    string EndPoint { get; }
}
