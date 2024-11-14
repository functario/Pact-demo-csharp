using Microsoft.AspNetCore.Authorization;

namespace CityService.Authentications;

public sealed record CityAuthorizationPolicy(
    string Name,
    Action<AuthorizationPolicyBuilder> PolicyBuilder
) { }
