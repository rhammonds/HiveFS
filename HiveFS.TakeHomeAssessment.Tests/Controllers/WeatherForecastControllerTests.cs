using HiveFS.Shared;
using HiveFS.TakeHomeAssessment.Controllers;
using HiveFS.WeatherData;
using HiveFS.WeatherData.Dtos;
using HiveFS.WeatherService;
using HiveFS.WeatherService.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace HiveFS.TakeHomeAssessment.Tests.Controllers;

public class WeatherForecastControllerTests
{
    readonly Mock<ILogger<WeatherForecastController>> _logger = new Mock<ILogger<WeatherForecastController>>();
    readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
    readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

    public WeatherForecastControllerTests()
    {
        _configuration
           .Setup(x => x["WeatherApiEndPoint"])
           .Returns("https://goweather.herokuapp.com");

        _configuration
           .Setup(x => x["CacheSeconds"])
           .Returns("30");
    }

    [Fact]
    public void GetCityForecast_NotFound_Fails()
    {
        //Arrange
        var apiResult = new ApiResult<WeatherForecast> { Data = null, Succeeded = false, HttpStatusCode = HttpStatusCode.NotFound };
        var weatherApi = new Mock<IWeatherApi>();
        weatherApi.Setup(r => r.GetCityForecast(It.IsAny<string>())).Returns(Task.FromResult(apiResult));
        var weatherForcastController = new WeatherForecastController(weatherApi.Object);

        //Act
        var result = weatherForcastController.GetCityForecast("NA");

        //Assert
        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.NotFound, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode);
        Assert.Equal("Data was not found.", ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value);
    }

    [Fact]
    public void GetCityForecast_Blank_Fails()
    {
        //Arrange
        var weatherRepository = new Mock<IWeatherRepository>();
        var weatherApi = new WeatherApi(weatherRepository.Object);
        var weatherForcastController = new WeatherForecastController(weatherApi);

        //Act
        var result = weatherForcastController.GetCityForecast("");

        //Assert
        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.BadRequest, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode);
        Assert.Equal("City is required.", ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value);
    }

    [Fact]
    public void GetCityForecast_Found_Success()
    {
        //Arrange
        var weatherForecast = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Today),
            TemperatureC = 100,
            Summary = ""
        };

        var apiResult = new ApiResult<WeatherForecast> { Data = weatherForecast, Succeeded = true, HttpStatusCode = System.Net.HttpStatusCode.OK };

        var weatherApi = new Mock<IWeatherApi>();
        weatherApi.Setup(r => r.GetCityForecast(It.IsAny<string>())).Returns(Task.FromResult(apiResult));
        var weatherForcastController = new WeatherForecastController(weatherApi.Object);

        //Act
        var result = weatherForcastController.GetCityForecast("Atlanta");

        //Assert
        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.OK, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode);
        Assert.Equal(212, ((WeatherForecast)((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value).TemperatureF);
    }

    [Fact]
    public void GetRandomForecast_Found_Success()
    {
        //Arrange
        var weatherApi = new Mock<IWeatherApi>();

        //Act
        var weatherForcastController = new WeatherForecastController(weatherApi.Object);
        
        //Assert
        var result = weatherForcastController.GetRandomForecasts();
        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.OK, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode);
    }
}
