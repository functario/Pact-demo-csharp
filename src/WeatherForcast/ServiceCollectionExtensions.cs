using DemoConfigurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Endpoint.Extensions;
using WeatherForcast.Clients.CityService.V1;
using WeatherForcast.Clients.TemperatureService.V1;
using WeatherForcast.Configurations;

namespace WeatherForcast;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeatherForcast(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        services.AddMinimalApi().AddSwagger();
        services.AddClients();
        services.AddWeatherForcastOptions().AddDemoConfigurations(context);

        services.AddAuthenticationCustom().AddAuthorizationCustom();
        return services;
    }

    public static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<AuthorizationDelegatingHandler>();
        services
            .AddHttpClient<ICityServiceClient, CityServiceClient>(
                (serviceProvider, client) =>
                {
                    var options = serviceProvider
                        .GetRequiredService<IOptions<WeatherForcastOptions>>()
                        .Value;

                    ArgumentNullException.ThrowIfNull(options, nameof(options));
                    client.BaseAddress = new Uri(options.CityServiceBaseAddress);
                }
            )
            .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

        services.AddHttpClient<ITemperatureServiceClient, TemperatureServiceClient>(
            (serviceProvider, client) =>
            {
                var options = serviceProvider
                    .GetRequiredService<IOptions<WeatherForcastOptions>>()
                    .Value;

                ArgumentNullException.ThrowIfNull(options, nameof(options));
                client.BaseAddress = new Uri(options.TemperatureServiceBaseAddress);
            }
        );

        return services;
    }

    public static IServiceCollection AddWeatherForcastOptions(this IServiceCollection services)
    {
        services
            .AddOptions<WeatherForcastOptions>()
            .BindConfiguration(WeatherForcastOptions.Section)
            .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddAuthenticationCustom(this IServiceCollection services)
    {
        var demoConfiguration = services
            .BuildServiceProvider()
            .GetRequiredService<DemoConfiguration>();

        var securityKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(demoConfiguration.AuthenticationKey)
        );

        services
            .AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:7252";
                options.Audience = "weatherforcastapi";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "authenticationservice",
                    ValidAudience = "weatherforcastapi",
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = securityKey,
                };
            });

        return services;
    }

    public static IServiceCollection AddAuthorizationCustom(this IServiceCollection services)
    {
        services.AddAuthorization();
        return services;
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition(
                "bearerAuth",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                }
            );

            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth"
                            }
                        },
                        []
                    }
                }
            );
        });
    }

    public static IServiceCollection AddMinimalApi(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            var demoConfiguration = services
                .BuildServiceProvider()
                .GetRequiredService<DemoConfiguration>();

            var demoOptions = demoConfiguration.GetJsonSerializerOptions();
            options.SerializerOptions.PropertyNamingPolicy = demoOptions.PropertyNamingPolicy;
            options.SerializerOptions.WriteIndented = demoOptions.WriteIndented;
            options.SerializerOptions.IncludeFields = demoOptions.IncludeFields;
            foreach (var converter in demoOptions.Converters)
            {
                options.SerializerOptions.Converters.Add(converter);
            }

            options.SerializerOptions.PropertyNameCaseInsensitive =
                demoOptions.PropertyNameCaseInsensitive;
            options.SerializerOptions.ReadCommentHandling = demoOptions.ReadCommentHandling;
        });

        return services.AddEndpointsApiExplorer().AddSwaggerGen().AddEndpoints();
    }
}
