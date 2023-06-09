using HiveFS.Shared;
using HiveFS.WeatherData.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace HiveFS.WeatherData;

public class WeatherRepository : IWeatherRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;
    private readonly string _weatherApiEndpoint;

    public WeatherRepository(IHttpClientFactory httpClientFactory, ILogger<WeatherRepository> logger, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _weatherApiEndpoint = configuration["WeatherApiEndPoint"] ;
    }
    public async Task<ApiResult<WeatherForecastDto>> GetCityForecast(string city)
    {
        var apiResult = new ApiResult<WeatherForecastDto>();
        var requestUri = $"{_weatherApiEndpoint}/weather/" + city;
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

        _logger.LogDebug("Calling Get Weather API with url: {url}", request.RequestUri);

        var httpClient = _httpClientFactory.CreateClient("WeatherRepository");

        using var response = await httpClient.SendAsync(request);

        var responseContent = response.Content.ReadAsStringAsync().Result;
        apiResult.HttpStatusCode = response.StatusCode;
        apiResult.Succeeded = response.StatusCode == HttpStatusCode.OK;

        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogDebug($"Get Weather API Call Results: {responseContent}");
            apiResult.Data = JsonSerializer.Deserialize<WeatherForecastDto>(responseContent);
        }
        else
        {
            _logger.LogError($"Get Weather API Call Failed with status code {response.StatusCode}: {responseContent}");
            apiResult.Message = response.StatusCode.ToString();
        }    

        return apiResult;
    }
}