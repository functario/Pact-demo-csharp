using TemperatureService.V1.Models;

namespace TemperatureService.V1.Temperatures;

public sealed record GetTemperaturesResponse(ICollection<Temperature> Temperatures) { }
