using MinimalApi.Endpoint;
using WeatherForcast.Clients.CityService.V1;
using WeatherForcast.Clients.TemperatureService.V1;
using WeatherForcast.Routes;
using WeatherForcast.V1.Forcasts.Models;

namespace WeatherForcast.V1.Forcasts.GetForcast;

public sealed class GetForcastEndPoint : IEndpoint<IResult, CancellationToken>
{
    private readonly ICityServiceClient _cityProviderClient;
    private readonly ITemperatureServiceClient _temperatureProviderClient;

    public GetForcastEndPoint(
        ICityServiceClient cityProviderClient,
        ITemperatureServiceClient temperatureProviderClient
    )
    {
        _cityProviderClient = cityProviderClient;
        _temperatureProviderClient = temperatureProviderClient;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        var route = $"/{EndPointRoutes.V1}/{EndPointRoutes.Forcasts}";
        app.MapGet(
                route,
                async (CancellationToken request) =>
                {
                    return await HandleAsync(request);
                }
            )
            .WithName(EndPointRoutes.Forcasts)
            .WithOpenApi()
            .WithTags(EndPointRoutes.Forcasts)
            .Produces(StatusCodes.Status200OK, typeof(GetForcastResponse));
        ;
    }

    public async Task<IResult> HandleAsync(CancellationToken cancellationToken)
    {
        var cityResponse = await _cityProviderClient.GetCities(cancellationToken);
        var temperatureResponse = await _temperatureProviderClient.GetTemperatures(
            cancellationToken
        );

        List<Forcast> forcasts = [];
        foreach (var city in cityResponse.Cities)
        {
            var temperature = temperatureResponse
                .Temperatures.Where(x =>
                    x.Location.CityName == city.Name && x.Location.Country == city.Country
                )
                .SingleOrDefault()
                ?.Value;

            if (temperature is not double validTemperature)
            {
                return TypedResults.NotFound(city);
            }

            forcasts.Add(new Forcast(city.Name, validTemperature, Units.Celsius));
        }

        return await Task.FromResult(TypedResults.Ok(forcasts));
    }
}
