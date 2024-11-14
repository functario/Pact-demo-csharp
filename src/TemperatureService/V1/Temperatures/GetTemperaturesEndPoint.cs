using MinimalApi.Endpoint;
using TemperatureService.APIConfigs;
using TemperatureService.Repositories;

namespace TemperatureService.V1.Temperatures;

public sealed class GetTemperaturesEndPoint : IEndpoint<IResult, CancellationToken>
{
    private readonly ITemperatureRepository _temperatureRepository;

    public GetTemperaturesEndPoint(ITemperatureRepository temperatureRepository)
    {
        _temperatureRepository = temperatureRepository;
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

    public async Task<IResult> HandleAsync(CancellationToken cancellationToken)
    {
        var temperatures = await _temperatureRepository.GetTemperatures(cancellationToken);
        var response = new GetTemperaturesResponse(temperatures);
        return await Task.FromResult(TypedResults.Ok(response));
    }
}
