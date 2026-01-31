using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoopNPour.Application.Authorization;
using PoopNPour.Infrastructure;
using PoopNPour.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Load database configuration from separate file (not in version control)
builder.Configuration.AddJsonFile("database.config.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddOpenApi();

// Add HTTP Context Accessor (required for authorization behavior)
builder.Services.AddHttpContextAccessor();

// Add Infrastructure (Database, Repositories, Identity)
builder.Services.AddInfrastructure(builder.Configuration);

// Add MediatR with pipeline behaviors
// TODO: Replace with a type from Application layer when adding features
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// Register authorization pipeline behavior
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("api/v1.json");
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

// Map endpoints here as they are created
// Example: app.Map{FeatureName}Endpoints();

app.Run();
