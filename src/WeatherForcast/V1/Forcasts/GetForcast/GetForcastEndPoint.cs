using MinimalApi.Endpoint;
using WeatherForcast.Routes;
using WeatherForcast.V1.Forcasts.DTOs;

namespace WeatherForcast.V1.Forcasts.GetForcast;

public sealed class GetForcastEndPoint : IEndpoint<IResult, CancellationToken>
{
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Naming",
        "CA1725:Parameter names should match base declaration",
        Justification = "<Pending>"
    )]
    public async Task<IResult> HandleAsync(CancellationToken _)
    {
        var forcasts = new GetForcastResponse([new Forcast("Paris", 10.5, Models.Units.Celsius)]);
        return await Task.FromResult(TypedResults.Ok(forcasts));
    }
}
