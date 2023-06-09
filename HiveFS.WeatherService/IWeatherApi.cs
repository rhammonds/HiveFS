using HiveFS.Shared;
using HiveFS.WeatherService.Models;

namespace HiveFS.WeatherService;

public interface IWeatherApi
{
    Task<ApiResult<WeatherForecast>> GetCityForecast(string city);
}