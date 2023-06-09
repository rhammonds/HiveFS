using HiveFS.Shared;
using HiveFS.WeatherData.Dtos;

namespace HiveFS.WeatherData;

public interface IWeatherRepository
{
    Task<ApiResult<WeatherForecastDto>> GetCityForecast(string city);
}