using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cityProvider = builder.AddProject<CityProvider>("cityprovider");
var temperatureProvider = builder.AddProject<TemperatureProvider>("temperatureprovider");
builder
    .AddProject<WeatherForcast>("weatherforcast")
    .WithReference(cityProvider)
    .WithReference(temperatureProvider);
var app = builder.Build();
app.Run();
