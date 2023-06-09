using HiveFS.FruitData;
using HiveFS.FruitService;
using HiveFS.FruitService.Models;
using HiveFS.Shared;
using HiveFS.TakeHomeAssessment.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog.Core;
using System.Net;

namespace HiveFS.TakeHomeAssessment.Tests.Controllers;

public class FruitControllerTests
{
    readonly Mock<ILogger<FruitController>> _logger = new Mock<ILogger<FruitController>>();
    readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
    readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

    public FruitControllerTests()
    {
        _configuration
           .Setup(x => x["WeatherApiEndPoint"])
           .Returns("https://goweather.herokuapp.com");

        _configuration
           .Setup(x => x["CacheSeconds"])
           .Returns("30");
    }

    [Fact]
    public void GetFruit_NotFound_Fails()
    {
        //Arrange
        var apiResult = new ApiResult<FruitNutrients> { Data = null, Succeeded = false, HttpStatusCode = HttpStatusCode.NotFound };
        var fruitApi = new Mock<IFruitApi>();
        fruitApi.Setup(r => r.GetFruit(It.IsAny<string>())).Returns(Task.FromResult(apiResult));
        var fruitController = new FruitController(fruitApi.Object, _memoryCache, _configuration.Object);
        
        //Act
        var result = fruitController.GetFruit("NA");
        
        //Assert
        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.NotFound, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode);
        Assert.Equal("Data was not found.", ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value);
    }

    [Fact]
    public void GetFruit_Blank_Fails()
    {
        //Arrange
     
        var fruitApi = new Mock<IFruitApi>();
        var fruitController = new FruitController(fruitApi.Object, _memoryCache, _configuration.Object); 
        
        //Act
        var result = fruitController.GetFruit("");

        //Assert
        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.BadRequest, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode);
        Assert.Equal("Fruit name is required.", ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value);
    }

    [Fact]
    public void GetFruit_BadRequest_Fails()
    {
        //Arrange
        var apiResult = new ApiResult<FruitNutrients> { Data = null, Succeeded = false, HttpStatusCode = HttpStatusCode.BadRequest };
        var fruitApi = new Mock<IFruitApi>();
        fruitApi.Setup(r => r.GetFruit(It.IsAny<string>())).Returns(Task.FromResult(apiResult));
        var fruitController = new FruitController(fruitApi.Object, _memoryCache, _configuration.Object); 

        //Act
        var result = fruitController.GetFruit("NA");

        //Assert
        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.BadRequest, ((Microsoft.AspNetCore.Mvc.StatusCodeResult)result.Result).StatusCode);        
    }

    [Fact]
    public void GetFruit_Unauthorized_Fails()
    {
        //Arrange
        var apiResult = new ApiResult<FruitNutrients> { Data = null, Succeeded = false, HttpStatusCode = HttpStatusCode.Unauthorized };
        var fruitApi = new Mock<IFruitApi>();
        fruitApi.Setup(r => r.GetFruit(It.IsAny<string>())).Returns(Task.FromResult(apiResult));
        var fruitController = new FruitController(fruitApi.Object, _memoryCache, _configuration.Object); 

        //Act
        var result = fruitController.GetFruit("NA");

        //Assert
        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.Unauthorized, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode);
        Assert.Equal("Not Authenticated.", ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value);
    }

    [Fact]
    public void GetFruit_InternalError_Fails()
    {
        //Arrange
        var apiResult = new ApiResult<FruitNutrients> { Data = null, Succeeded = false, HttpStatusCode = HttpStatusCode.InternalServerError };
        var fruitApi = new Mock<IFruitApi>();
        fruitApi.Setup(r => r.GetFruit(It.IsAny<string>())).Returns(Task.FromResult(apiResult));
        var fruitController = new FruitController(fruitApi.Object, _memoryCache, _configuration.Object); 

        //Act
        var result = fruitController.GetFruit("NA");

        //Assert
        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.InternalServerError, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode);
        Assert.Equal("Internal Error Occurred.", ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value);
    }

    [Fact]
    public void GetFruit_ValidFruit_Success()
    {
        //Arrange
        var expectedFruitNutrients = new FruitNutrients
        {
            Calories = 123,
            Carbohydrates = 456
        };
       
        var fruitApi = new Mock<IFruitApi>(); 
        var fruitController = new FruitController(fruitApi.Object, _memoryCache, _configuration.Object); 
        var apiResult = new ApiResult<FruitNutrients> { Data = expectedFruitNutrients, Succeeded = true, HttpStatusCode = HttpStatusCode.OK };

        fruitApi.Setup(r => r.GetFruit(It.IsAny<string>())).Returns(Task.FromResult(apiResult));
        //Act
        var result = fruitController.GetFruit("Atlanta");

        //Assert
        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.OK, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode);
        Assert.Equal(123, ((FruitNutrients)((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value).Calories);
    }

}
