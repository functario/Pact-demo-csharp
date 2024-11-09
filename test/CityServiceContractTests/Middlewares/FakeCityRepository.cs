using CityService.Repositories;
using CityService.V1.Models;

namespace CityServiceContractTests.Middlewares;

public class FakeCityRepository : ICityRepository
{
    private readonly ICollection<City> _cities;

    public FakeCityRepository()
    {
        _cities = [];
    }

    public ICollection<City> DataSetCities => _cities;

    public Task<ICollection<City>> GetCities(CancellationToken _)
    {
        return Task.FromResult(DataSetCities);
    }
}
