using DemoConfigurations;
using JWTGenerator;
using Microsoft.AspNetCore.Http;

namespace CityServiceContractTests.Middlewares;

public class AuthorizationMiddleware
{
    private const string AuthorizationHeaderKey = "Authorization";
    private readonly RequestDelegate _next;
    private readonly DemoConfiguration _demoConfiguration;

    public AuthorizationMiddleware(RequestDelegate next, DemoConfiguration demoConfiguration)
    {
        _next = next;
        _demoConfiguration = demoConfiguration;
    }

    public async Task Invoke(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        if (context.Request.Headers.ContainsKey(AuthorizationHeaderKey))
        {
            AuthorizationHeader(context.Request);
        }
        else
        {
            await _next(context);
        }
    }

    private void AuthorizationHeader(HttpRequest request)
    {
        var token = TokenGenerator.GenerateToken(_demoConfiguration.AuthenticationKey);

        request.Headers.Authorization = $"Bearer {token}";
    }
}
