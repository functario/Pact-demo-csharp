using CityService.Repositories;
using CityService.Routes;
using MinimalApi.Endpoint;

namespace CityService.V1.Cities;

public sealed class GetCitiesEndpoint : IEndpoint<IResult, CancellationToken>
{
    private readonly ICityRepository _cityRepository;

    public GetCitiesEndpoint(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

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

    public async Task<IResult> HandleAsync(CancellationToken cancellationToken)
    {
        var cities = await _cityRepository.GetCities(cancellationToken);
        var response = new GetCitiesResponse(cities);
        return TypedResults.Ok(response);
    }
}
