namespace TemperatureService.V1.Models;

public sealed record Temperature(
    double Value,
    Units Unit,
    DateTimeOffset RecordDate,
    Location Location
) { }
