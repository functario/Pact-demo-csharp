using WeatherForcast.Clients.TemperatureService.V1.Models;

namespace WeatherForcastContractTests.Fixtures.TemperatureServiceFixtures;

public static class LocationFixtures
{
    public static Location SeoulLocation =>
        new("Seoul", "South Korea", new GeoCoordinate(48.8575, 2.3514, -0.3757));

    public static Location NewYorkLocation =>
        new("New York", "USA", new GeoCoordinate(48.8575, 2.3514, -0.3757));

    public static Location QuebecLocation =>
        new("Quebec", "Canada", new GeoCoordinate(46.829853, -73.935242, 1.0245));
}
