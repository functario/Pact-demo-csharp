using CityService.APIConfigs;
using CityService.Authentications;
using CityService.Repositories;
using MinimalApi.Endpoint;

namespace CityService.V1.Cities;

public sealed class GetCitiesEndpoint : IEndpoint<IResult, CancellationToken>
{
    private readonly ICityRepository _cityRepository;
    private readonly PolicyNames _policyNames;

    public GetCitiesEndpoint(
        ICityRepository cityRepository,
        [FromKeyedServices(PolicyNames.KeyedServiceName)] PolicyNames policyNames
    )
    {
        _cityRepository = cityRepository;
        _policyNames = policyNames;
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
            .RequireAuthorization(_policyNames.Names)
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
