using WeatherForcast.Clients.CityService.V1.Models;

namespace WeatherForcast.Clients.CityService.V1.DTOs;

public sealed record GetCitiesResponse(ICollection<City> Cities) { }
