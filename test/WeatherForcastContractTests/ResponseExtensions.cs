using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
