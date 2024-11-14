namespace CityService.Authentications;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "Optionnal parameters"
)]
public sealed record PolicyNames(params string[] Names)
{
    public const string KeyedServiceName = "citypolicy";
}
