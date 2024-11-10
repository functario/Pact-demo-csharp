using AuthenticationService.Routes;
using DemoConfigurations;
using Microsoft.AspNetCore.Authentication.BearerToken;
using MinimalApi.Endpoint;

namespace AuthenticationService.V1.Authenticate;

public sealed class AuthenticateEndPoint
    : IEndpoint<IResult, AccessTokenResponse, CancellationToken>
{
    private readonly DemoConfiguration _demoConfiguration;

    public AuthenticateEndPoint(DemoConfiguration demoConfiguration)
    {
        _demoConfiguration = demoConfiguration;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        var route = $"/{EndPointRoutes.V1}/{EndPointRoutes.Authenticate}";
        app.MapPost(
                route,
                async (AccessTokenResponse request, CancellationToken cancellationToken) =>
                {
                    return await HandleAsync(request, cancellationToken);
                }
            )
            .WithName(EndPointRoutes.Authenticate)
            .WithOpenApi()
            .WithTags(EndPointRoutes.Authenticate)
            .Produces(StatusCodes.Status200OK, typeof(AuthenticateResponse))
            .Produces(StatusCodes.Status401Unauthorized, typeof(AuthenticateResponse));
        ;
    }

    public async Task<IResult> HandleAsync(
        AccessTokenResponse request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var secretKey = _demoConfiguration.AuthenticationKey;
        var token = request.AccessToken.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

        var (isValid, error) = TokenValidator.ValidateToken(token, secretKey);

        return await Task.FromResult(TypedResults.Ok(new AuthenticateResponse(isValid, error)));
    }
}
