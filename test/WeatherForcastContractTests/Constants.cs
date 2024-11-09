namespace WeatherForcastContractTests;

internal static class Constants
{
    public static string PactDir => Path.Combine("../../../../pacts");
    public const string ProtocolHttp = "http";
    public const string Host = "127.0.0.1";
    public const int CityServicePort = 7429;
    public static string CityServiceBaseAddress => $"{ProtocolHttp}://{Host}:{CityServicePort}";
}
