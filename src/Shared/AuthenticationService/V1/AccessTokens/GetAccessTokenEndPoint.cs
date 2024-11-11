using AuthenticationService.Routes;
using DemoConfigurations;
using JWTGenerator;
using Microsoft.AspNetCore.Authentication.BearerToken;
using MinimalApi.Endpoint;

namespace AuthenticationService.V1.AccessTokens;

public sealed class GetAccessTokenEndPoint : IEndpoint<IResult, HttpContext, CancellationToken>
{
    private readonly DemoConfiguration _demoConfiguration;

    public GetAccessTokenEndPoint(DemoConfiguration demoConfiguration)
    {
        _demoConfiguration = demoConfiguration;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        var route = $"/{EndPointRoutes.V1}/{EndPointRoutes.AccessTokens}";
        app.MapGet(
                route,
                async (HttpContext httpContext, CancellationToken request) =>
                {
                    return await HandleAsync(httpContext, request);
                }
            )
            .WithName(EndPointRoutes.AccessTokens)
            .WithOpenApi()
            .WithTags(EndPointRoutes.AccessTokens)
            .Produces(StatusCodes.Status200OK, typeof(AccessTokenResponse));
        ;
    }

    public async Task<IResult> HandleAsync(
        HttpContext httpContext,
        CancellationToken cancellationToken
    )
    {
        var accessToken = TokenGenerator.GenerateToken(_demoConfiguration.AuthenticationKey);
        var refreshToken = accessToken;
        var bearerToken = new AccessTokenResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = 3600
        };

        return await Task.FromResult(TypedResults.Ok(bearerToken));
    }
}
