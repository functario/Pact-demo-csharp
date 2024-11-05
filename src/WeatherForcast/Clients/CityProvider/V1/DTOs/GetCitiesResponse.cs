using WeatherForcast.Clients.CityProvider.V1.Models;

namespace WeatherForcast.Clients.CityProvider.V1.DTOs;

public sealed record GetCitiesResponse(ICollection<City> Cities) { }
