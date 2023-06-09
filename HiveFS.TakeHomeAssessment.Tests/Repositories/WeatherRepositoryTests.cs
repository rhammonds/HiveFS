using HiveFS.WeatherData;
using HiveFS.WeatherService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace HiveFS.TakeHomeAssessment.Tests.Repositories;

public class WeatherForecastTests
{
    Mock<IConfiguration> _configuration;
    Mock<ILogger<WeatherRepository>> _logger;

    public WeatherForecastTests()
    {
        //ignore logger
        _logger = new Mock<ILogger<WeatherRepository>>();

        var configurationSectionMock = new Mock<IConfigurationSection>();
        _configuration = new Mock<IConfiguration>();

        //mock endpoint configuration
        configurationSectionMock
           .Setup(x => x.Value)
           .Returns("https://goweather.herokuapp.com");

        _configuration
           .Setup(x => x.GetSection("WeatherApiEndPoint"))
           .Returns(configurationSectionMock.Object);
    }



    [Fact]
    public async void GetCityForecast_ValidCity_Success()
    {
        //Arrange
        var data = @"{""temperature"":""25 °C"",""wind"":""6 km/h"",""description"":""Partly cloudy"",""forecast"":[{""day"":""1"",""temperature"":""24 °C"",""wind"":""10 km/h""},{""day"":""2"",""temperature"":""+26 °C"",""wind"":""17 km/h""},{""day"":""3"",""temperature"":""23 °C"",""wind"":""20 km/h""}]}";
        var _httpClientFactory = Helper.GetClientFactory(HttpStatusCode.OK, "WeatherRepository", data);
        var weatherRepository = new WeatherRepository(_httpClientFactory.Object, _logger.Object, _configuration.Object);

        //Act
        var query = new WeatherApi(weatherRepository);
        var result = await query.GetCityForecast("Atlanta");

        //Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Data);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
    }

    [Fact]
    public async void GetCityForecast_ValidCity_Fail()
    {
        //Arrange
        var _httpClientFactory = Helper.GetClientFactory(HttpStatusCode.NotFound, "WeatherRepository", "Failed");
        var weatherRepository = new WeatherRepository(_httpClientFactory.Object, _logger.Object, _configuration.Object);

        //Act
        var query = new WeatherApi(weatherRepository);
        var result = await query.GetCityForecast("Atlanta");

        //Assert
        Assert.False(result.Succeeded);
        Assert.Null(result.Data);
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode); 
    }
    [Fact]
    public async void GetCityForecast_BlankCity_Fail()
    {
        //Arrange
        var _httpClientFactory = Helper.GetClientFactory(HttpStatusCode.NotFound, "WeatherRepository", "Failed");
        var weatherRepository = new WeatherRepository(_httpClientFactory.Object, _logger.Object, _configuration.Object);

        //Act
        var query = new WeatherApi(weatherRepository);
        var result = await query.GetCityForecast("");

        //Assert
        Assert.False(result.Succeeded);
        Assert.Null(result.Data);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
    }
}

