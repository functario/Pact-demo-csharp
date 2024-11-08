using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Builder;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cityProvider = builder.AddProject<CityProvider>("cityprovider");
builder.AddProject<WeatherForcast>("weatherforcast").WithReference(cityProvider);
builder.AddProject<TemperatureProvider>("temperatureprovider");
var app = builder.Build();
app.Run();
