using TemperatureService.Repositories;
using TemperatureService.V1.Models;

namespace TemperatureServiceContractTests.Middlewares;

internal sealed class FakeTemperatureRepository : ITemperatureRepository
{
    private readonly ICollection<Temperature> _temperatures;

    public FakeTemperatureRepository(TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider, nameof(timeProvider));
        _temperatures = [];
    }

    public ICollection<Temperature> DataSetTemperatures => _temperatures;

    public Task<ICollection<Temperature>> GetTemperatures(CancellationToken _)
    {
        return Task.FromResult(DataSetTemperatures);
    }
}
