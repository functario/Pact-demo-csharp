namespace CityServiceContractTests;

public static class Constants
{
    public const string ProtocolHttp = "http";
    public const string Host = "127.0.0.1";
    public const int CityServicePort = 7848;
    public static string CityServiceBaseAddress => $"{ProtocolHttp}://{Host}:{CityServicePort}";
}
