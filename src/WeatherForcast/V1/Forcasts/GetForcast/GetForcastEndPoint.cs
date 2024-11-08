using MinimalApi.Endpoint;
using WeatherForcast.Clients.CityProvider.V1;
using WeatherForcast.Clients.TemperatureProvider.V1;
using WeatherForcast.Routes;
using WeatherForcast.V1.Forcasts.Models;

namespace WeatherForcast.V1.Forcasts.GetForcast;

public sealed class GetForcastEndPoint : IEndpoint<IResult, CancellationToken>
{
    private readonly ICityProviderClient _cityProviderClient;
    private readonly ITemperatureProviderClient _temperatureProviderClient;

    public GetForcastEndPoint(
        ICityProviderClient cityProviderClient,
        ITemperatureProviderClient temperatureProviderClient
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
            forcasts.Add(new Forcast(city.Name, 10.5, Units.Celsius));
        }

        return await Task.FromResult(TypedResults.Ok(forcasts));
    }
}
