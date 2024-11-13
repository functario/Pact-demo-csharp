using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<AuthenticationService>("authenticationservice");
var cityservice = builder.AddProject<CityService>("cityservice");

var temperatureService = builder.AddProject<TemperatureService>("temperatureservice");

builder
    .AddProject<WeatherForcast>("weatherforcast")
    .WithReference(cityservice)
    .WithReference(temperatureService);

var app = builder.Build();
app.Run();
