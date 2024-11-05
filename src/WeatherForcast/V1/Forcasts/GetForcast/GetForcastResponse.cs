using WeatherForcast.V1.Forcasts.DTOs;

namespace WeatherForcast.V1.Forcasts.GetForcast;

public sealed record GetForcastResponse(ICollection<Forcast> Forcasts) { }
