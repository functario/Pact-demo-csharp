using MinimalApi.Endpoint;
using TemperatureProvider.Routes;
using TemperatureProvider.V1.Models;

namespace TemperatureProvider.V1.Temperatures;

public sealed class GetTemperaturesEndPoint : IEndpoint<IResult, CancellationToken>
{
    private readonly TimeProvider _timeProvider;

    public GetTemperaturesEndPoint(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        var route = $"/{EndPointRoutes.V1}/{EndPointRoutes.Temperatures}";
        app.MapGet(
                route,
                async (CancellationToken request) =>
                {
                    return await HandleAsync(request);
                }
            )
            .WithName(EndPointRoutes.Temperatures)
            .WithOpenApi()
            .WithTags(EndPointRoutes.Temperatures)
            .Produces(StatusCodes.Status200OK, typeof(GetTemperaturesResponse));
        ;
    }

    public async Task<IResult> HandleAsync(CancellationToken _)
    {
        var recordDate = _timeProvider.GetUtcNow();
        var geoCoordinates = new GeoCoordinate(48.8575, 2.3514, 1.0245);
        var location = new Location("Paris", "France", geoCoordinates);
        var temperatures = new GetTemperaturesResponse(
            [new Temperature(18.5, Units.Celsius, recordDate, location)]
        );
        return await Task.FromResult(TypedResults.Ok(temperatures));
    }
}
