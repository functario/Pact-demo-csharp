using System.Net.Http.Headers;
using WeatherForcastContractTests.Fixtures.AuthenticationServiceFixtures;

namespace WeatherForcastContractTests.Support;

internal sealed class FakeAuthorizationDelegatingHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var token = AuthenticationFixtures.ValidOldToken;
        if (!string.IsNullOrEmpty(token))
        {
            // Add the Authorization header to the request
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        // Proceed with the request
        return base.SendAsync(request, cancellationToken);
    }
}
