using WeatherForcast.Clients.TemperatureService.V1.Models;

namespace WeatherForcastContractTests.Fixtures.TemperatureServiceFixtures;

public static class LocationFixtures
{
    public static Location ParisLocation =>
        new("Paris", "France", new GeoCoordinate(48.8575, 2.3514, -0.3757));
    public static Location MelbourneLocation =>
        new("Melbourne", "Australia", new GeoCoordinate(-37.840935, 144.946457, 0.0778));
    public static Location QuebecLocation =>
        new("Quebec", "Canada", new GeoCoordinate(46.829853, -73.935242, 1.0245));
}
