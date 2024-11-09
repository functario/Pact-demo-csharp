using WeatherForcast.Clients.TemperatureService.V1.Models;

namespace WeatherForcast.Clients.TemperatureService.V1.DTOs;

public sealed record GetTemperaturesResponse(ICollection<Temperature> Temperatures) { }
