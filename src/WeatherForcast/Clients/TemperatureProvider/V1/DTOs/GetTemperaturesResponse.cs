using WeatherForcast.Clients.CityProvider.V1.Models;
using WeatherForcast.Clients.TemperatureProvider.V1.Models;

namespace WeatherForcast.Clients.TemperatureProvider.V1.DTOs;

public sealed record GetTemperaturesResponse(ICollection<Temperature> Temperatures) { }
