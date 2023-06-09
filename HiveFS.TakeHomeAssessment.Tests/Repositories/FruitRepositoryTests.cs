using HiveFS.FruitData;
using HiveFS.FruitService;
using HiveFS.WeatherData;
using HiveFS.WeatherService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;

namespace HiveFS.TakeHomeAssessment.Tests.Repositories;

public class FruitRepositoryTests
{
    Mock<IConfiguration> _configuration;
    Mock<ILogger<FruitRepository>> _logger;
    public FruitRepositoryTests()
    {
        //ignore logger
        _logger = new Mock<ILogger<FruitRepository>>();

        var configurationSectionMock = new Mock<IConfigurationSection>();
        _configuration = new Mock<IConfiguration>();

        //mock endpoint configuration
        configurationSectionMock
           .Setup(x => x.Value)
           .Returns("https://goweather.herokuapp.com");

        _configuration
           .Setup(x => x.GetSection("FruitApiEndPoint"))
           .Returns(configurationSectionMock.Object);
    }

    [Fact]
    public async void GetFruit_ValidFruit_Success()
    {
        //Arrange
        var data = @"{""name"":""Apple"",""id"":6,""family"":""Rosaceae"",""order"":""Rosales"",""genus"":""Malus"",""nutritions"":{""calories"":52,""fat"":0.4,""sugar"":10.3,""carbohydrates"":11.4,""protein"":0.3}}";
        var _httpClientFactory = Helper.GetClientFactory(HttpStatusCode.OK, "FruitRepository", data);
        var fruitRepository = new FruitRepository(_httpClientFactory.Object, _logger.Object, _configuration.Object);

        //Act
        var query = new FruitApi(fruitRepository);
        var result = await query.GetFruit("Apple");

        //Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Data);
        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
        Assert.Equal(11.4m, result.Data.Carbohydrates);
    }

    [Fact]
    public async void GetFruit_ValidFruit_Failed()
    {
        //Arrange
        var _httpClientFactory = Helper.GetClientFactory(HttpStatusCode.NotFound, "FruitRepository", "Failed");
        var fruitRepository = new FruitRepository(_httpClientFactory.Object, _logger.Object, _configuration.Object);

        //Act
        var query = new FruitApi(fruitRepository);
        var result = await query.GetFruit("Apple");

        //Assert
        Assert.False(result.Succeeded);
        Assert.Null(result.Data);
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
    }

    [Fact]
    public async void GetFruit_BlankFruit_Failed()
    {
        //Arrange
        var _httpClientFactory = Helper.GetClientFactory(HttpStatusCode.NotFound, "FruitRepository", "Failed");
        var fruitRepository = new FruitRepository(_httpClientFactory.Object, _logger.Object, _configuration.Object);

        //Act
        var query = new FruitApi(fruitRepository);
        var result = await query.GetFruit("");

        //Assert
        Assert.False(result.Succeeded);
        Assert.Null(result.Data);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
    }
}
