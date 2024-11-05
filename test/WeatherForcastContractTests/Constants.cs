using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace WeatherForcastContractTests;

internal static class Constants
{
    public const string ProtocolHttp = "http";
    public const string Host = "127.0.0.1";
    public const string CityProvider = "CityProvider";
    public const int CityProviderPort = 7429;
    public static string CityProviderBaseAddress => $"{ProtocolHttp}://{Host}:{CityProviderPort}";
    public const string WeatherForcast = "WeatherForcast";
}
