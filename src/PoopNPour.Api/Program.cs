using MediatR;
using Microsoft.AspNetCore.Identity;
using PoopNPour.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Load database configuration from separate file (not in version control)
builder.Configuration.AddJsonFile("database.config.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddOpenApi();

// Add Infrastructure (Database, Repositories, Identity)
builder.Services.AddInfrastructure(builder.Configuration);

// Add MediatR
// TODO: Replace with a type from Application layer when adding features
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

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

// Map endpoints here as they are created
// Example: app.Map{FeatureName}Endpoints();

app.Run();
