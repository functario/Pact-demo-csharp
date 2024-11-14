using System.Text.Json;
using System.Text.Json.Serialization;
using CityService.Autenthication;
using CityService.Repositories;
using DemoConfigurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Endpoint.Extensions;

namespace CityService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCityService(
        this IServiceCollection services,
        HostBuilderContext context,
        StartupOptions? startupOptions
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddMinimalApi()
            .AddRepositories(startupOptions?.InjectedCityRepository)
            .AddDemoConfigurations(context)
            .AddSwagger();

        services.AddAuthenticationCustom().AddAuthorizationCustom();
        return services;
    }

    internal static void AddSwagger(this IServiceCollection services)
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

    internal static IServiceCollection AddAuthenticationCustom(this IServiceCollection services)
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

    internal static IServiceCollection AddAuthorizationCustom(
        this IServiceCollection services,
        params NamedAuthorizationPolicy[] nameAuthorizationPolicies
    )
    {
        if (nameAuthorizationPolicies is not null && nameAuthorizationPolicies.Length > 0)
        {
            services.AddAuthorization(x => AddPolicies(x, nameAuthorizationPolicies));
        }

        // Default
        return services.AddAuthorization();
    }

    private static void AddPolicies(
        AuthorizationOptions authorizationOptions,
        NamedAuthorizationPolicy[] nameAuthorizationPolicies
    )
    {
        foreach (var nameAuthorizationPolicy in nameAuthorizationPolicies)
        {
            authorizationOptions.AddPolicy(
                nameAuthorizationPolicy.Name,
                nameAuthorizationPolicy.Policy
            );
        }
    }

    internal static IServiceCollection AddRepositories(
        this IServiceCollection services,
        ICityRepository? injectedCityRepository
    )
    {
        // Inject the repository for test.
        // Probably safer than conditional environment since it must be instanciated.
        // Also the fake repository is not hardcoded in the source of the service.
        if (injectedCityRepository is not null)
        {
            return services.AddScoped(_ => injectedCityRepository);
        }

        return services.AddScoped<ICityRepository, CityRepository>();
    }

    internal static IServiceCollection AddMinimalApi(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.IncludeFields = true;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        });

        return services.AddEndpointsApiExplorer().AddSwaggerGen().AddEndpoints();
    }
}
