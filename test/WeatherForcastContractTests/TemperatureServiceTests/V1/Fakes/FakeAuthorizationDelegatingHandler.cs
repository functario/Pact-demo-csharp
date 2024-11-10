using System.Net.Http.Headers;

namespace WeatherForcastContractTests.TemperatureServiceTests.V1.Fakes;

public sealed class FakeAuthorizationDelegatingHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        // Retrieve the Bearer token from the HttpContext
        //var token =
        //    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MzEyNjE3MjgsImlzcyI6ImF1dGhlbnRpY2F0aW9uc2VydmljZSIsImF1ZCI6IndlYXRoZXJmb3JjYXN0YXBpIn0.TMwRk-3BbhheWcWlKH4yPDbmEKgcqb5RAUipZYbH5BA";

        //var token =
        //"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im1vam9oIiwic3ViIjoibW9qb2giLCJqdGkiOiJkNzA1YTZkNyIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjQzMjc1IiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzMDciLCJodHRwOi8vbG9jYWxob3N0OjUxODYiLCJodHRwczovL2xvY2FsaG9zdDo3MjA3Il0sIm5iZiI6MTczMTIzNzQxNSwiZXhwIjoxNzM5MTg2MjE1LCJpYXQiOjE3MzEyMzc0MTUsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.WawK_9mRnt4uQQP62ozI2JELFYXkUGchUtpLB1-O19o";

        var token = "toto";
        if (!string.IsNullOrEmpty(token))
        {
            // Add the Authorization header to the request
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Proceed with the request
        return base.SendAsync(request, cancellationToken);
    }
}
