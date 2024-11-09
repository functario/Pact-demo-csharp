using CityService.V1.Models;

namespace CityService.Repositories;

public class FakeCityRepository : ICityRepository
{
    public Task<ICollection<City>> GetCities()
    {
        var cities = new List<City>()
        {
            new("New York", "USA"),
            new("Berlin", "Germany"),
            new("India", "Pune")
        };

        return Task.FromResult<ICollection<City>>(cities);
    }
}
