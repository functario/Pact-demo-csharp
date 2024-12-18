﻿// This file is used by Code Analysis to maintain SuppressMessage
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
    "Style",
    "IDE0290:Use primary constructor",
    Justification = "Use explicit and readonly field"
)]
[assembly: SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Test nomenclature",
    Scope = "namespaceanddescendants",
    Target = "~N:TemperatureServiceContractTests.V1"
)]
[assembly: SuppressMessage(
    "Design",
    "CA1052:Static holder types should be Static or NotInheritable",
    Justification = "Load at runtime",
    Scope = "type",
    Target = "~T:TemperatureServiceContractTests.TestsConfigurations.TemperatureServiceContractTestsCollection"
)]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "Part of xunit DSL",
    Scope = "type",
    Target = "~T:TemperatureServiceContractTests.TestsConfigurations.TemperatureServiceContractTestsCollection"
)]
