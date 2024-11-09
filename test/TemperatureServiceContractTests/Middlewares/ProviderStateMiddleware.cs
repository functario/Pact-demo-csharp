using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PactReferences;
using PactReferences.ProviderStates;
using TemperatureService.Repositories;
using TemperatureService.V1.Models;

//using ProvidersPactStates;

namespace TemperatureServiceContractTests.Middlewares;

public sealed class ProviderStateMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TimeProvider _timeProvider;
    private readonly FakeTemperatureRepository _temperatureRepository;
    private readonly IDictionary<string, Action> _providerStates;
    internal const string ProviderStatesPath = "provider-states";

    public ProviderStateMiddleware(
        RequestDelegate next,
        TimeProvider timeProvider,
        ITemperatureRepository temperatureRepository
    )
    {
        _next = next;
        _timeProvider = timeProvider;
        _temperatureRepository =
            temperatureRepository as FakeTemperatureRepository
            ?? throw new InvalidCastException(
                $"Could not cast {nameof(temperatureRepository)} to {nameof(FakeTemperatureRepository)}."
            );

        // Map state with Actions
        _providerStates = new Dictionary<string, Action>
        {
            { TemperatureServiceStates.SomeTemperaturesExist.State, Create3Temperatures }
        };
    }

    private void Create3Temperatures()
    {
        var recordDate = _timeProvider.GetUtcNow();

        List<Temperature> temperatures =
        [
            new(
                35,
                Units.Celsius,
                recordDate,
                new(
                    "FakeMelbourne",
                    "FakeAustralia",
                    new GeoCoordinate(-37.840935, 144.946457, 0.0778)
                )
            )
        ];

        _temperatureRepository.DataSetTemperatures.AddRange(temperatures);
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
