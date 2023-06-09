using HiveFS.Shared;
using HiveFS.WeatherData;
using HiveFS.WeatherService.Models;
using System.Net;

namespace HiveFS.WeatherService;

public class WeatherApi : IWeatherApi
{
    private readonly IWeatherRepository _weatherRepository;

    public WeatherApi(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
    }

    public async Task<ApiResult<WeatherForecast>> GetCityForecast(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return new ApiResult<WeatherForecast> { Succeeded = false, HttpStatusCode = HttpStatusCode.BadRequest }; ;
        }

        var data = await _weatherRepository.GetCityForecast(city);
        var result = new ApiResult<WeatherForecast> { Succeeded = data.Succeeded, HttpStatusCode = data.HttpStatusCode };
        if (data.Succeeded)
        {
            result.Data = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                Summary = data.Data.description,
                TemperatureC = int.Parse(data.Data.temperature.Split(' ')[0])
            };
        };
        return result;
    }
}
