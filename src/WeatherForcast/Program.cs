using dotenv.net;
using MinimalApi.Endpoint.Extensions;
using WeatherForcast;

var builder = WebApplication.CreateBuilder(args);

// Aspire
builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();

// Load appsettings and environment variables.
DotEnv.Fluent().WithTrimValues().WithOverwriteExistingVars().Load();
builder.Configuration.AddJsonFile($"appsettings.json", optional: false).AddEnvironmentVariables();

// Register Services
builder.Host.ConfigureServices((context, services) => services.AddWeatherForcast(context));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapEndpoints();
app.MapDefaultEndpoints();
app.Run();
