using CityService.Autenthication;
using CityService.Repositories;

namespace CityService;

public readonly record struct StartupOptions(
    ICityRepository? InjectedCityRepository = null,
    ICollection<NamedAuthorizationPolicy>? NameAuthorizationPolicies = null
) { }
