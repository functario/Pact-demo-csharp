using WeatherForcast.Clients.CityService.V1.Models;

namespace WeatherForcastContractTests.Fixtures.CityServiceFixtures;

public static class CityFixtures
{
    public static City Seoul => new("Seoul", "South Korea");
    public static City NewYork => new("New York", "USA");
    public static City Quebec => new("Québec", "Canada");

    public static ICollection<City> SetOf3Cities => [Seoul, NewYork, Quebec];
}
