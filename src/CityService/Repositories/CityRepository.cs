using CityService.V1.Models;

namespace CityService.Repositories;

public sealed class CityRepository : ICityRepository
{
    private readonly ICollection<City> _cities =
    [
        new("Paris", "France"),
        new("Berlin", "Germany"),
        new("Melbourne", "Australia")
    ];

    public Task<ICollection<City>> GetCities(CancellationToken _)
    {
        return Task.FromResult(_cities);
    }
}
