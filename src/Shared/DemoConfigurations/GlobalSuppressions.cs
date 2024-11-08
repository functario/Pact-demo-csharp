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
    "Design",
    "CA1024:Use properties where appropriate",
    Justification = "<Pending>",
    Scope = "member",
    Target = "~M:DemoConfiguration.Configurations.DemoConfiguration.GetJsonSerializerOptions~System.Text.Json.JsonSerializerOptions"
)]
[assembly: SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "Allowed for environment variables",
    Scope = "type",
    Target = "~T:DemoConfigurations.EnvironmentVars"
)]