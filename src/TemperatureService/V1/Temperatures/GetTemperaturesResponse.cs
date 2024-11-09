using TemperatureProvider.V1.Models;

namespace TemperatureProvider.V1.Temperatures;

public sealed record GetTemperaturesResponse(ICollection<Temperature> Temperatures) { }
