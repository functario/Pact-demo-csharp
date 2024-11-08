namespace TemperatureProvider.V1.Models;

public sealed record Location(string CityName, string Country, GeoCoordinate GeoCoordinate) { }
