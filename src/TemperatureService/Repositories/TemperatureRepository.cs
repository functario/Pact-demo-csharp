using TemperatureService.V1.Models;

namespace TemperatureService.Repositories;

public sealed class TemperatureRepository : ITemperatureRepository
{
    private readonly ICollection<Temperature> _temperatures;

    public TemperatureRepository(TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider, nameof(timeProvider));
        var recordDate = timeProvider.GetUtcNow();

        _temperatures =
        [
            new(
                35,
                Units.Celsius,
                recordDate,
                new("Melbourne", "Australia", new GeoCoordinate(-37.840935, 144.946457, 0.0778))
            )
        ];
    }

    public Task<ICollection<Temperature>> GetTemperatures(CancellationToken _)
    {
        return Task.FromResult(_temperatures);
    }
}
