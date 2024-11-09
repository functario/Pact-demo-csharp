using CityService.V1.Models;

namespace CityService.Repositories;

public interface ICityRepository
{
    Task<ICollection<City>> GetCities();
}
