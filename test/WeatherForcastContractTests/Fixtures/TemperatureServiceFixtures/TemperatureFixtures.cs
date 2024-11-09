using WeatherForcast.Clients.TemperatureService.V1.Models;

namespace WeatherForcastContractTests.Fixtures.TemperatureServiceFixtures;

public static class TemperatureFixtures
{
    // Note: Try to cover all the possible value ranges related to your domain.
    // - Always have 3 values for collection
    // - Mixed positive and negative values for unsigned numbers.
    public static ICollection<Temperature> SetOf3CelsiusTemperatures =>
        [
            new Temperature(
                15.1,
                Units.Celsius,
                DateTimeOffFixtures.DateInYear2025,
                LocationFixtures.ParisLocation
            ),
            new Temperature(
                32,
                Units.Celsius,
                DateTimeOffFixtures.DateInYear2025,
                LocationFixtures.MelbourneLocation
            ),
            new Temperature(
                -25,
                Units.Celsius,
                DateTimeOffFixtures.DateInYear2025,
                LocationFixtures.QuebecLocation
            ),
        ];
}
