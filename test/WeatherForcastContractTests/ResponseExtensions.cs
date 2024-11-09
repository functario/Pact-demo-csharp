using System.Text.Json;

namespace WeatherForcastContractTests;

public static class ResponseExtensions
{
    public static dynamic? ToLowerDynamic<T>(
        this T response,
        JsonSerializerOptions jsonSerializerOptions
    )
    {
        return JsonSerializer.Deserialize<dynamic>(
            JsonSerializer.Serialize(response, jsonSerializerOptions)
        );
    }
}
