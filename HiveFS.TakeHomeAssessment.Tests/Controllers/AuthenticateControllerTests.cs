using AuthenticationandAuthorization.Controllers;
using HiveFS.TakeHomeAssessment.Controllers;
using HiveFS.TakeHomeAssessment.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace HiveFS.TakeHomeAssessment.Tests.Controllers;

public class AuthenticateControllerTests
{
    readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
    readonly Mock<ILogger<FruitController>> _logger = new Mock<ILogger<FruitController>>();
    public AuthenticateControllerTests()
    {
        _configuration
           .Setup(x => x["Jwt:Key"])
           .Returns("Thisismysecretkey");

        _configuration
           .Setup(x => x["Jwt:Issuer"])
           .Returns("https://localhost");

        _configuration
           .Setup(x => x["userid"])
           .Returns("test");

        _configuration
           .Setup(x => x["password"])
           .Returns("test");
    }

    [Fact]
    public void Login_Valid_Success()
    {
        //Arrange
        var loginModel = new LoginModel { UserId = "test", Password = "test" };
        var weatherForcastController = new AuthenticateController(_configuration.Object, _logger.Object);
        var result = weatherForcastController.Login(loginModel);

        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.OK, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode);
    }

    [Fact]
    public void Login_InValid_Fails()
    {
        //Arrange
        var loginModel = new LoginModel { UserId = "invalid", Password = "invalid" };
        var weatherForcastController = new AuthenticateController(_configuration.Object, _logger.Object);
        var result = weatherForcastController.Login(loginModel);

        Assert.NotNull(result.Result);
        Assert.Equal((int)HttpStatusCode.Unauthorized, ((Microsoft.AspNetCore.Mvc.StatusCodeResult)result.Result).StatusCode);
    }
}