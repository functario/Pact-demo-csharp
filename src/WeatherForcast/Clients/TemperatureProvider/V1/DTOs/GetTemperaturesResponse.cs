using WeatherForcast.Clients.TemperatureProvider.V1.Models;

namespace WeatherForcast.Clients.TemperatureProvider.V1.DTOs;

public sealed record GetTemperaturesResponse(ICollection<Temperature> Temperatures) { }
