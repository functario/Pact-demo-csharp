using System.Net.Http.Headers;

namespace WeatherForcast.Clients.CityService.V1;

public sealed class AuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        ArgumentNullException.ThrowIfNull(_httpContextAccessor, nameof(_httpContextAccessor));
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        ArgumentNullException.ThrowIfNull(
            _httpContextAccessor.HttpContext,
            nameof(_httpContextAccessor.HttpContext)
        );

        // Retrieve the Bearer token from the HttpContext
        var token = _httpContextAccessor
            .HttpContext.Request.Headers.Authorization.ToString()
            .Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

        if (!string.IsNullOrEmpty(token))
        {
            // Add the Authorization header to the request
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Proceed with the request
        return base.SendAsync(request, cancellationToken);
    }
}
