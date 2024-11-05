using WeatherForcast.V1.Forcasts.Models;

namespace WeatherForcast.V1.Forcasts.DTOs;

public record Forcast(string City, double Temperature, Units Unit) { }
