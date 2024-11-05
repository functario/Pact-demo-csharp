﻿using MinimalApi.Endpoint;
using WeatherForcast.Clients.CityProvider.V1;
using WeatherForcast.Routes;
using WeatherForcast.V1.Forcasts.Models;

namespace WeatherForcast.V1.Forcasts.GetForcast;

public sealed class GetForcastEndPoint : IEndpoint<IResult, CancellationToken>
{
    private readonly ICityProviderClient _cityProviderClient;

    public GetForcastEndPoint(ICityProviderClient cityProviderClient)
    {
        _cityProviderClient = cityProviderClient;
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

        List<Forcast> forcasts = [];
        foreach (var city in cityResponse.Cities)
        {
            forcasts.Add(new Forcast(city.Name, 10.5, Models.Units.Celsius));
        }

        return await Task.FromResult(TypedResults.Ok(forcasts));
    }
}