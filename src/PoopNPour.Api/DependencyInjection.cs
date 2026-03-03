using Microsoft.OpenApi;
using PoopNPour.Api.Common;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for configuring API layer services
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Poop & Pour API",
                Version = "v1",
                Description = "Poop & Pour API for its web & mobile application.",
                Contact = new OpenApiContact
                {
                    Name = "Poop & Pour Team",
                    Email = "info@poopnpour.co.za"
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Paste your JWT token below. Swagger will add the 'Bearer ' prefix automatically.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });

            options.OperationFilter<EndpointDocumentationFilter>();  // Summary/Description
            options.OperationFilter<AutoResponseTypesFilter>();      // Automatic response types
            options.SchemaFilter<SchemaExampleFilter>();             // User-friendly request examples
        });

        return services;
    }
}
