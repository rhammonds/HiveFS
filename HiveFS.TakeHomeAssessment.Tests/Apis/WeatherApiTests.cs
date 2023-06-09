using HiveFS.Shared;
using HiveFS.WeatherData;
using HiveFS.WeatherData.Dtos;
using HiveFS.WeatherService;
using Moq;
using System.Net;

namespace HiveFS.TakeHomeAssessment.Tests.Apis
{
    public class WeatherApiTests
    {
        [Fact]
        public async void GetCityForecast_Found_ReturnsSuccess()
        {
            //Arrange
            var expectedF = 212;
            var weatherForecastDto = new WeatherForecastDto
            {
                forecast = new List<Forecast>
                {
                     new Forecast
                     {
                          day = "100",
                           temperature = "",
                           wind = ""
                     }
                },
                description = "",
                temperature = "100",
                wind = ""
            };

            var apiResult = new ApiResult<WeatherForecastDto> { Data = weatherForecastDto, Succeeded = true, HttpStatusCode = System.Net.HttpStatusCode.OK };

            var weatherRepository = new Mock<IWeatherRepository>();
            weatherRepository.Setup(r => r.GetCityForecast(It.IsAny<string>())).Returns(Task.FromResult(apiResult));

            //Act
            var query = new WeatherApi(weatherRepository.Object);
            var result = await query.GetCityForecast("Atlanta");

            //Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedF, result.Data.TemperatureF);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Today), result.Data.Date);
            weatherRepository.Verify(m => m.GetCityForecast(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void GetCityForecast_NotFound_ReturnsNotFound()
        {
            //Arrange
            var apiResult = new ApiResult<WeatherForecastDto> { Data = null, Succeeded = false, HttpStatusCode = System.Net.HttpStatusCode.NotFound };
            var weatherRepository = new Mock<IWeatherRepository>();
            weatherRepository.Setup(r => r.GetCityForecast(It.IsAny<string>())).Returns(Task.FromResult(apiResult));

            //Act
            var query = new WeatherApi(weatherRepository.Object);
            var result = await query.GetCityForecast("a");

            //Assert
            Assert.False(result.Succeeded);
            Assert.Null(result.Data);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
            weatherRepository.Verify(m => m.GetCityForecast(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void GetCityForecast_EmptyParm_ReturnsBadRequest()
        {
            //Arrange
            var apiResult = new ApiResult<WeatherForecastDto> { Data = null, Succeeded = false, HttpStatusCode = HttpStatusCode.NotFound };
            var weatherRepository = new Mock<IWeatherRepository>();
            weatherRepository.Setup(r => r.GetCityForecast(It.IsAny<string>())).Returns(Task.FromResult(apiResult));

            //Act
            var query = new WeatherApi(weatherRepository.Object);
            var result = await query.GetCityForecast("");

            //Assert
            Assert.False(result.Succeeded);
            Assert.Null(result.Data);
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
            weatherRepository.Verify(m => m.GetCityForecast(It.IsAny<string>()), Times.Never);
        }

    }
}

