using CityService.V1.Models;

namespace CityService.V1.Cities;

public sealed record GetCitiesResponse(ICollection<City> Cities) { }
