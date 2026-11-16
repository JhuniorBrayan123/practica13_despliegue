using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Lab13_JhuniorGutierrez.Application;
using Lab13_JhuniorGutierrez.Infrastructure.Configuration;
using Lab13_JhuniorGutierrez.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace Lab13_JhuniorGutierrez.Api.Configuration;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);

        // Enums travel as text ("Abierto", "EnProceso", "Cerrado"); numbers are rejected.
        services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));
        services.AddEndpointsApiExplorer();

        services.AddJwtAuthentication(configuration);
        services.AddSwaggerDocumentation();

        return services;
    }

    private static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwt = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                  ?? throw new InvalidOperationException("The 'Jwt' configuration section is missing.");

        if (Encoding.UTF8.GetByteCount(jwt.Key) < 32)
            throw new InvalidOperationException("Jwt:Key must be at least 32 bytes long for HMAC-SHA256.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();
    }

    private static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Lab13_JhuniorGutierrez Ticketing API",
                Version = "v1",
                Description = "Ticket support API built with Hexagonal (Clean) Architecture, JWT, Repository and Unit of Work."
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Paste only the token returned by /api/auth/login (without the 'Bearer ' prefix)."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });

            foreach (var assembly in new[] { Assembly.GetExecutingAssembly(), typeof(DependencyInjection).Assembly })
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);
            }
        });
    }
}
