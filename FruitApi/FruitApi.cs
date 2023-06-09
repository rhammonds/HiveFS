using HiveFS.FruitData;
using HiveFS.FruitService.Models;
using HiveFS.Shared;
using System.Net;

namespace HiveFS.FruitService
{
    public class FruitApi : IFruitApi
    {
        private readonly IFruitRepository _fruitRepository;
        public FruitApi(IFruitRepository fruitRepository)
        {
            _fruitRepository = fruitRepository;
        }

        public async Task<ApiResult<FruitNutrients>> GetFruit(string fruitName)
        {
            if (string.IsNullOrWhiteSpace(fruitName))
            {
                return new ApiResult<FruitNutrients> { Succeeded = false, HttpStatusCode = HttpStatusCode.BadRequest }; ;
            }

            var data = await _fruitRepository.GetFruit(fruitName);
            var result = new ApiResult<FruitNutrients> { Succeeded = data.Succeeded, HttpStatusCode = data.HttpStatusCode };
            if (data.Succeeded)
            {
                result.Data = new FruitNutrients
                {
                    Calories = data.Data.nutritions.calories,
                    Carbohydrates = data.Data.nutritions.carbohydrates
                };
            }

            return result;
        }
    }
}