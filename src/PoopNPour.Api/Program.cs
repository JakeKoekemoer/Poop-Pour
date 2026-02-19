using PoopNPour.Api.Common;
using PoopNPour.Api.Middleware;
using PoopNPour.Infrastructure;
using PoopNPour.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Load database configuration from separate file (not in version control)
builder.Configuration.AddJsonFile("database.config.json", optional: true, reloadOnChange: true);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddApiServices();

var app = builder.Build();

// Global exception handling (must be first in pipeline)
app.UseExceptionHandling();

// Enable static files (for custom Swagger CSS/JS)
app.UseStaticFiles();

// Enable CORS
app.UseCors();

app.UseHttpsRedirection();

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

// Serve Vue app for all non-API routes (supports Vue Router history mode)
app.MapFallbackToFile("index.html");

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

// Make Program class accessible for functional tests (WebApplicationFactory<Program>)
public partial class Program { }
