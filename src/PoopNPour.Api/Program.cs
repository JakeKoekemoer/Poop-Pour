using MediatR;
using Microsoft.OpenApi.Models;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Behaviours;
using PoopNPour.Api.Common;
using PoopNPour.Api.Middleware;
using PoopNPour.Infrastructure;
using PoopNPour.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Load database configuration from separate file (not in version control)
builder.Configuration.AddJsonFile("database.config.json", optional: false, reloadOnChange: true);

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
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

    // Add JWT Bearer authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Paste your JWT token below. Swagger will add the 'Bearer ' prefix automatically.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });

    // Add operation filters (.NET 10 Swashbuckle approach)
    options.OperationFilter<EndpointDocumentationFilter>();  // Summary/Description
    options.OperationFilter<AutoResponseTypesFilter>();      // Automatic response types
    options.SchemaFilter<SchemaExampleFilter>();             // User-friendly request examples

});

// Add HTTP Context Accessor (required for authorization behavior)
builder.Services.AddHttpContextAccessor();

// Add CORS for Swagger UI
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Infrastructure (Database, Repositories, Identity)
builder.Services.AddInfrastructure(builder.Configuration);

// Add MediatR with pipeline behaviors
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(PoopNPour.Application.Authentication.Commands.AuthenticateUserCommand).Assembly);
});

// Register MediatR pipeline behaviors (order matters!)
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));  // Logging
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));        // Authorization

var app = builder.Build();

// Global exception handling (must be first in pipeline)
app.UseExceptionHandling();

// Enable static files (for custom Swagger CSS/JS)
app.UseStaticFiles();

// Enable CORS
app.UseCors();

app.UseHttpsRedirection();

// Add Identity middleware
app.UseAuthentication();
app.UseAuthorization();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

// Map all endpoints automatically using reflection
// NB: Run before Swagger UI is configured
app.MapEndpointGroups();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI (custom theme via static index.html with Tailwind + Alpine.js)
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Poop & Pour API v1");
        options.RoutePrefix = "swagger";
    });
}

app.Run();
