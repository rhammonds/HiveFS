using HiveFS.Shared;
using HiveFS.WeatherService;
using HiveFS.WeatherService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HiveFS.TakeHomeAssessment.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : HiveFsBaseController
{
    private readonly IWeatherApi _weatherApi;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecastController(IWeatherApi weatherApi)
    {
        _weatherApi = weatherApi;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WeatherForecast))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetRandomForecasts")]
    public async Task<IActionResult> GetRandomForecasts()
    {
        return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray());
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WeatherForecast))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{city}")]
    public async Task<IActionResult> GetCityForecast(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return BadRequest("City is required.");
        }
         
        return MapResult(await _weatherApi.GetCityForecast(city));
    }


}
