namespace WeatherForcastContractTests;

internal static class Constants
{
    public static string PactDir => Path.Combine("../../../../pacts");
    public const string ProtocolHttp = "http";
    public const string Host = "127.0.0.1";
    public const int CityServicePort = 7429;
    public const int TemperatureServicePort = 7430;
    public static string CityServiceBaseAddress => $"{ProtocolHttp}://{Host}:{CityServicePort}";
    public static string TemperatureServiceBaseAddress =>
        $"{ProtocolHttp}://{Host}:{TemperatureServicePort}";
}
