using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProvidersPactStates;

//using ProvidersPactStates;

namespace CityServiceContractTests.Middleware;

public sealed class ProviderStateMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDictionary<string, Action> _providerStates;

    public ProviderStateMiddleware(RequestDelegate next)
    {
        _next = next;

        // Map state with Actions
        _providerStates = new Dictionary<string, Action>
        {
            { CityServiceStates.SomeCitiesExist.State, Create3Cities }
        };
    }

    private void Create3Cities()
    {
        // TODO: implement repository.
    }

    public async Task Invoke(HttpContext context)
    {
        // Warning: Debugging this section with breakpoint may timeout silently.
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        if (
            context.Request.Path.StartsWithSegments(
                "/provider-states",
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            await this.HandleProviderStatesRequest(context);
            await context.Response.WriteAsync(string.Empty);
        }
        else
        {
            await this._next(context);
        }
    }

    private async Task HandleProviderStatesRequest(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.OK;

        if (
            context.Request.Method.Equals(
                HttpMethod.Post.ToString(),
                StringComparison.OrdinalIgnoreCase
            )
            && context.Request.Body != null
        )
        {
            var jsonRequestBody = string.Empty;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                jsonRequestBody = await reader.ReadToEndAsync();
            }

            var providerState = JsonConvert.DeserializeObject<ProviderStateFunc>(jsonRequestBody);

            //A null or empty provider state key must be handled
            if (providerState != null && !string.IsNullOrEmpty(providerState.State))
            {
                _providerStates[providerState.State].Invoke();
            }
        }
    }
}
