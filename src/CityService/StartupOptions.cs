using CityService.Repositories;

namespace CityService;

public readonly record struct StartupOptions(ICityRepository? InjectedCityRepository = null) { }
