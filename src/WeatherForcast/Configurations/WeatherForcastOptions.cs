using System.ComponentModel.DataAnnotations;

namespace WeatherForcast.Configurations;

public class WeatherForcastOptions
{
    public const string Section = "WeatherForcastOptions";

    [Required(
        AllowEmptyStrings = false,
        ErrorMessage = $"{nameof(CityServiceBaseAddress)} is required"
    )]
    public string CityServiceBaseAddress { get; set; } = string.Empty;

    [Required(
        AllowEmptyStrings = false,
        ErrorMessage = $"{nameof(TemperatureServiceBaseAddress)} is required"
    )]
    public string TemperatureServiceBaseAddress { get; set; } = string.Empty;
}
