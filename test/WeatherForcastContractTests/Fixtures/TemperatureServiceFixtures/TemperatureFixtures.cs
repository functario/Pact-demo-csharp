using WeatherForcast.Clients.TemperatureService.V1.Models;

namespace WeatherForcastContractTests.Fixtures.TemperatureServiceFixtures;

public static class TemperatureFixtures
{
    // Note: Try to cover all the possible value ranges related to your domain.
    // - Always have 3 values for collection
    // - Mixed positive and negative values for unsigned numbers.
    // - Mixed integer and decimal for double.
    // - Strings with space, different number of chars, etc.
    public static ICollection<Temperature> SetOf3CelsiusTemperatures =>
        [
            new Temperature(
                15.1,
                Units.Celsius,
                DateTimeOffFixtures.DateInYear2025,
                LocationFixtures.NewYorkLocation
            ),
            new Temperature(
                32,
                Units.Celsius,
                DateTimeOffFixtures.DateInYear2025,
                LocationFixtures.SeoulLocation
            ),
            new Temperature(
                -25,
                Units.Celsius,
                DateTimeOffFixtures.DateInYear2025,
                LocationFixtures.QuebecLocation
            ),
        ];
}
