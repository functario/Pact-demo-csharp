using System.Text.Json;

namespace WeatherForcastContractTests;

public static class ResponseExtensions
{
    private static readonly JsonSerializerOptions s_snakeCaseLowerOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = false
        };

    public static dynamic? ToLowerDynamic<T>(this T response)
    {
        return JsonSerializer.Deserialize<dynamic>(
            JsonSerializer.Serialize(response, s_snakeCaseLowerOptions)
        );
    }
}
