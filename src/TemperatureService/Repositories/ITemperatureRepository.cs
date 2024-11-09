using TemperatureService.V1.Models;

namespace TemperatureService.Repositories;

public interface ITemperatureRepository
{
    Task<ICollection<Temperature>> GetTemperatures(CancellationToken cancellationToken);
}
