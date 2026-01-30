using MediatR;
using PoopNPour.Api.Endpoints;
using PoopNPour.Application.WeatherForecasts.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetWeatherForecastQuery).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("api/v1.json");
}

app.UseHttpsRedirection();

// Map endpoints
app.MapWeatherForecastsEndpoints();

app.Run();
