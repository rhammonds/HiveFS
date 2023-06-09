using HiveFS.FruitService.Models;
using HiveFS.Shared;

namespace HiveFS.FruitService;

public interface IFruitApi
{
    public Task<ApiResult<FruitNutrients>> GetFruit(string fruitName);
}
