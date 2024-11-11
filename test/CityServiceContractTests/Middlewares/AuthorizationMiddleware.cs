using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace CityServiceContractTests.Middlewares;

public partial class AuthorizationMiddleware
{
    private const string AuthorizationHeaderKey = "Authorization";
    private readonly RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        if (context.Request.Headers.ContainsKey(AuthorizationHeaderKey))
        {
            AuthorizationHeader(context.Request);
            await this._next(context);
        }
        else
        {
            UnauthorizedResponse(context);
        }
    }

    private static string AuthorizationHeader(HttpRequest request)
    {
        var authorizationHeader = request.Headers.Authorization.FirstOrDefault();
        if (authorizationHeader is null)
            return "";
        var match = BearerRegex().Match(authorizationHeader);
        var value = match.Groups[1].Value;
        // TODO: Return a valid token
        //request.Headers.Authorization =
        //
        return value;
    }

    private static void UnauthorizedResponse(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }

    [GeneratedRegex("Bearer (.*)")]
    private static partial Regex BearerRegex();
}
