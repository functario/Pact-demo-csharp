namespace CityProviderContractTests;

public static class Constants
{
    public static string PactDir => Path.Combine("../../../../pacts");
    public const string ProtocolHttp = "http";
    public const string Host = "127.0.0.1";
    public const string CityProvider = "CityProvider";
    public const int CityProviderPort = 7848;
    public static string CityProviderBaseAddress => $"{ProtocolHttp}://{Host}:{CityProviderPort}";
}
