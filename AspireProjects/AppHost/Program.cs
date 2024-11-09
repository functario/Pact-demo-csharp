using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cityservice = builder.AddProject<CityService>("cityservice");
var temperatureProvider = builder.AddProject<TemperatureService>("temperatureservice");

builder
    .AddProject<WeatherForcast>("weatherforcast")
    .WithReference(cityservice)
    .WithReference(temperatureProvider);

var app = builder.Build();
app.Run();
