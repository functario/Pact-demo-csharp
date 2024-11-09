// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Reliability",
    "CA2007:Consider calling ConfigureAwait on the awaited task",
    Justification = "Not production code"
)]
[assembly: SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "Instance disposed by Xunit.DependencyInjection",
    Scope = "member",
    Target = "~M:CityServiceContractTests.ServiceCollectionExtensions.AddCityService(Microsoft.Extensions.DependencyInjection.IServiceCollection)~Microsoft.Extensions.DependencyInjection.IServiceCollection"
)]
[assembly: SuppressMessage(
    "Style",
    "IDE0290:Use primary constructor",
    Justification = "Use explicit and readonly field"
)]
[assembly: SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test nomenclature",
    Scope = "namespaceanddescendants",
    Target = "~N:CityServiceContractTests.V1"
)]
[assembly: SuppressMessage(
    "Naming",
    "CA1725:Parameter names should match base declaration",
    Justification = "Fake implementation",
    Scope = "type",
    Target = "~T:CityServiceContractTests.Middlewares.FakeCityRepository"
)]
