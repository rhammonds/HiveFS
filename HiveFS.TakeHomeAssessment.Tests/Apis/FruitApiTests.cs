using HiveFS.FruitData;
using HiveFS.FruitData.Dtos;
using HiveFS.FruitService;
using HiveFS.FruitService.Models;
using HiveFS.Shared;
using Moq;
using System.Net;

namespace HiveFS.TakeHomeAssessment.Tests.Apis
{
    public class FruitApiTests
    {
        [Fact]
        public async void GetFruit_Found_ReturnsSuccess()
        {
            //Arrange
            var expectedFruitNutrients = new FruitNutrients
            {
                Calories = 123,
                Carbohydrates = 456
            };
            var FruitDto = new FruitDto
            {
                family = "",
                genus = "",
                id = 1,
                name = "",
                order = "",
                nutritions = new Nutritions
                {
                    calories = 123,
                    carbohydrates = 456,
                    fat = 1,
                    protein = 1,
                    sugar = 1,
                }
            };
            var apiResult = new ApiResult<FruitDto> { Data = FruitDto, Succeeded = true, HttpStatusCode = System.Net.HttpStatusCode.OK };

            var fruitRepository = new Mock<IFruitRepository>();
            fruitRepository.Setup(r => r.GetFruit(It.IsAny<string>())).Returns(Task.FromResult(apiResult));

            //Act
            var query = new FruitApi(fruitRepository.Object);
            var result = await query.GetFruit("Banana");

            //Assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedFruitNutrients.Calories, result.Data.Calories );
            Assert.Equal(expectedFruitNutrients.Carbohydrates, result.Data.Carbohydrates);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            fruitRepository.Verify(m => m.GetFruit(It.IsAny<string>()), Times.Once);

        }

        [Fact]
        public async void GetFruit_NotFound_ReturnsNotFound()
        {
            //Arrange
            var apiResult = new ApiResult<FruitDto> { Data = null, Succeeded = false, HttpStatusCode = System.Net.HttpStatusCode.NotFound };
            var fruitRepository = new Mock<IFruitRepository>();
            fruitRepository.Setup(r => r.GetFruit(It.IsAny<string>())).Returns(Task.FromResult(apiResult));

            //Act
            var query = new FruitApi(fruitRepository.Object);
            var result = await query.GetFruit("a");

            //Assert
            Assert.False(result.Succeeded);
            Assert.Null(result.Data);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
            fruitRepository.Verify(m => m.GetFruit(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void GetFruit_EmptyParm_ReturnsBadRequest()
        {
            //Arrange
            var apiResult = new ApiResult<FruitDto> { Data = null, Succeeded = false, HttpStatusCode = System.Net.HttpStatusCode.NotFound };
            var fruitRepository = new Mock<IFruitRepository>();
            fruitRepository.Setup(r => r.GetFruit(It.IsAny<string>())).Returns(Task.FromResult(apiResult));

            //Act
            var query = new FruitApi(fruitRepository.Object);
            var result = await query.GetFruit("");

            //Assert
            Assert.False(result.Succeeded);
            Assert.Null(result.Data);
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
            fruitRepository.Verify(m => m.GetFruit(It.IsAny<string>()), Times.Never);
        }


    }
}
