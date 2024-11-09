using CityService.Routes;
using MinimalApi.Endpoint;

namespace CityService.V1.Cities;

public sealed class GetCitiesEndpoint : IEndpoint<IResult, CancellationToken>
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        var route = $"/{EndPointRoutes.V1}/{EndPointRoutes.Cities}";
        app.MapGet(
                route,
                async (CancellationToken request) =>
                {
                    return await HandleAsync(request);
                }
            )
            .WithName(EndPointRoutes.Cities)
            .WithOpenApi()
            .WithTags(EndPointRoutes.Cities)
            .Produces(StatusCodes.Status200OK, typeof(GetCitiesResponse));
        ;
    }

    public async Task<IResult> HandleAsync(CancellationToken _)
    {
        var cities = new GetCitiesResponse([new("Paris", "France")]);
        return await Task.FromResult(TypedResults.Ok(cities));
    }
}
