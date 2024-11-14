using Microsoft.AspNetCore.Authorization;

namespace CityService.Autenthication;

public readonly record struct NamedAuthorizationPolicy(
    string Name,
    Action<AuthorizationPolicyBuilder> PolicyBuilder
) { }
