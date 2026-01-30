using MediatR;
using PoopNPour.Application.WeatherForecasts.Models;
using PoopNPour.Application.WeatherForecasts.Queries;

namespace PoopNPour.Api.Endpoints;

public static class WeatherForecastsEndpoints
{
    public static void MapWeatherForecastsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weatherforecasts")
            .WithTags("WeatherForecasts");

        group.MapGet("/", GetWeatherForecast)
            .WithName("GetWeatherForecast")
            .Produces<IEnumerable<WeatherForecast>>();
    }

    private static async Task<IResult> GetWeatherForecast(IMediator mediator)
    {
        var query = new GetWeatherForecastQuery();
        var forecast = await mediator.Send(query);
        return Results.Ok(forecast);
    }
}
