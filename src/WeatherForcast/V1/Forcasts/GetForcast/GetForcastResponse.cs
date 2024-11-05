using WeatherForcast.V1.Forcasts.Models;

namespace WeatherForcast.V1.Forcasts.GetForcast;

public sealed record GetForcastResponse(ICollection<Forcast> Forcasts) { }
