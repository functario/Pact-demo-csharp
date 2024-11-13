using System.Net;
using System.Text;
using CityService.Repositories;
using CityService.V1.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PactReferences;
using PactReferences.ProviderStates;

namespace CityServiceContractTests.Middlewares;

public sealed class ProviderStateMiddleware
{
    private readonly RequestDelegate _next;
    private readonly FakeCityRepository _cityRepository;
    private readonly IDictionary<string, Action> _providerStates;
    internal const string ProviderStatesPath = "provider-states";

    public ProviderStateMiddleware(RequestDelegate next, ICityRepository cityRepository)
    {
        _next = next;
        _cityRepository =
            cityRepository as FakeCityRepository
            ?? throw new InvalidCastException(
                $"Could not cast {nameof(cityRepository)} to {nameof(FakeCityRepository)}."
            );

        // Map state with Actions
        _providerStates = new Dictionary<string, Action>
        {
            { CityServiceStates.SomeCitiesExist.State, Create3Cities }
        };
    }

    public async Task Invoke(HttpContext context)
    {
        // Warning: Debugging this section with breakpoint may timeout silently.
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        if (
            context.Request.Path.StartsWithSegments(
                $"/{ProviderStatesPath}",
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            await HandleProviderStatesRequest(context);
            await context.Response.WriteAsync(string.Empty);
        }
        else
        {
            await _next(context);
        }
    }

    private void Create3Cities()
    {
        var cities = new List<City>()
        {
            new("Fake City1", "Fake_Country1"),
            new("FakeCity2", "Fake Country 2"),
            new("Fake_City3", "FakeCountry3")
        };

        _cityRepository.DataSetCities.AddRange(cities);
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
