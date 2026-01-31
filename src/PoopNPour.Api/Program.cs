using MediatR;
using Microsoft.OpenApi.Models;
using PoopNPour.Application.Authorization;
using PoopNPour.Api.Endpoints;
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
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
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

// Register authorization pipeline behavior
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));

var app = builder.Build();

// Enable static files (for custom Swagger CSS/JS)
app.UseStaticFiles();

// Enable CORS
app.UseCors();

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

// Map endpoints
app.MapAuthenticationEndpoints();
app.MapUsersEndpoints();

app.Run();
