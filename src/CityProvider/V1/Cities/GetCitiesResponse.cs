using CityProvider.V1.Models;

namespace CityProvider.V1.Cities;

public sealed record GetCitiesResponse(ICollection<City> Cities) { }
